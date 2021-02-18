using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;

public class HiveEnemy : EnemyBase
{
    //FSM
    /// <summary>
    /// 0 if rooted, 1 if normal
    /// </summary>
    float _movementModifier = 1f;
    //engage
    public float distanceToEngage;
    public float directionMovementCheckTime;
    float _currentDirectionTime;

    public float hoverSpeed;
    bool _hoverLeft;

    //Attack
    public float attackCooldown;
    float _currentAttackCooldown;

    public float attackRange;

    private EventFSM<Inputs> _stateMachine;
    public enum Inputs { EnemyFound, EnemyInAttackRange, StateEnd, Die };

    Entity _currentTarget;

    MiniHiveShip[] _ships;

    public Entity CurrentTarget
    {
        get
        {
            if (_currentTarget == null) _currentTarget = _player;
            return _currentTarget;
        }
        protected set => _currentTarget = value;
    }

    public string GetCurrentState()
    {
        return _stateMachine.Current.Name;
    }

    protected override void Start()
    {
        base.Start();
        _ships = GetComponentsInChildren<MiniHiveShip>(true).OrderBy(x => x.name).ToArray();
        _ships[0].InitializeShip(maxHP);
        _ships[1].InitializeShip(maxHP / 2);
        _ships[2].InitializeShip(maxHP / 3);
        CurrentTarget = _player;
        InitFsm();
    }

    void InitFsm()
    {
        //-----------------------------------------STATE CREATE-------------------------------------------//
        var idle = new State<Inputs>("Idle");
        var engage = new State<Inputs>("Engage");
        var attack = new State<Inputs>("Attack");
        var death = new State<Inputs>("Death");

        StateConfigurer.Create(idle)
            .SetTransition(Inputs.EnemyFound, engage)
            .SetTransition(Inputs.Die, death)
            .Done();

        StateConfigurer.Create(engage)
            .SetTransition(Inputs.EnemyInAttackRange, attack)
            .SetTransition(Inputs.Die, death)
            .Done();

        StateConfigurer.Create(attack)
           .SetTransition(Inputs.StateEnd, engage)
           .SetTransition(Inputs.Die, death)
           .Done();

        StateConfigurer.Create(death).Done();

        //-----------------------------------------STATE SET-------------------------------------------//

        //idle
        idle.OnEnter += x =>
        {

        };

        //engage
        engage.OnEnter += x =>
        {
            var dir = CurrentTarget.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;
        };

        engage.OnFixedUpdate += () =>
        {
            var dir = CurrentTarget.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;

            //move to player if distance > attackRange/2
            if (Vector3.Distance(CurrentTarget.transform.position, transform.position) >= attackRange / 2)
            {
                _rb.MovePosition(_rb.position + (Vector2)dir * (movementSpeed * _movementModifier) * Time.fixedDeltaTime);
            }
        };

        engage.OnExit += x =>
        {

        };

        //attack
        attack.OnEnter += x =>
        {

        };

        attack.OnFixedUpdate += () =>
        {
            var movMultiplicator = _hoverLeft ? -1 : 1;
            _rb.MovePosition(_rb.position + (Vector2)transform.right * (hoverSpeed * movMultiplicator * _movementModifier) * Time.fixedDeltaTime);
            _currentDirectionTime += Time.fixedDeltaTime;
            if (_currentDirectionTime >= directionMovementCheckTime)
            {
                _hoverLeft = !_hoverLeft;
                _currentDirectionTime = 0f;
            }

            if (_currentAttackCooldown >= attackCooldown)
            {
                //shoot dir transform.up
                Shoot();
                _currentAttackCooldown = 0;
            }
            else
            {
                _currentAttackCooldown += Time.deltaTime;
                var dir = CurrentTarget.transform.position - transform.position;
                transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;
            }
        };

        attack.OnExit += x =>
        {

        };

        //death
        death.OnEnter += x =>
        {
            Destroy(gameObject);
        };

        //-----------------------------------------FSM INIT-------------------------------------------//
        _stateMachine = new EventFSM<Inputs>(idle);
    }

    private void ProcessInput(Inputs inp)
    {
        _stateMachine.SendInput(inp);
    }

    protected override void Update()
    {
        CheckSensors();
        _stateMachine.Update();
    }

    protected override void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        OnTakeDamage();
        foreach (var item in _ships.Where(x => x != null))
        {
            item.TakeDamage(dmg);
        }
    }

    public void OnTakeDamage()
    {
        if (CurrentHP <= 0)
        {
            Die();
        }
        else ProcessInput(Inputs.EnemyFound);
    }

    public void ClearDestroyedShips(MiniHiveShip clear)
    {
        _ships = _ships.Where(x => x.gameObject != clear.gameObject).ToArray();
    }

    public void Die()
    {
        ProcessInput(Inputs.Die);
    }

    public void AttackEnd()
    {
        ProcessInput(Inputs.StateEnd);
    }

    //Sensor checking
    void CheckSensors()
    {
        if (GetCurrentState() == "Death") return;

        if (GetCurrentState() == "Idle")
        {
            if (Vector3.Distance(CurrentTarget.transform.position, transform.position) <= distanceToEngage)
            {
                ProcessInput(Inputs.EnemyFound);
            }
        }

        if (Vector3.Distance(CurrentTarget.transform.position, transform.position) <= attackRange && _currentAttackCooldown <= 0)
        {
            ProcessInput(Inputs.EnemyInAttackRange);
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < _ships.Where(x => x.gameObject != null).Count(); i++)
        {
            BaseProyectile instancedProjectile;

            if (_isMindControlled && _inyectedSeeds.Any())
            {
                instancedProjectile = comboSystem.DefineCombo(_inyectedSeeds);
            }
            else
            {
                instancedProjectile = GameObject.Instantiate(defaultProjectile, _ships[i].muzzle.transform.position, Quaternion.identity);
            }
            instancedProjectile.SpawnProjectile(_ships[i].muzzle.transform.position, transform.up, this);
        }

    }

    protected override void StunHandler(bool state)
    {
        _movementModifier = state ? 0 : 1;
    }

    protected override void MindControlHandler(bool state)
    {
        if (state)
        {
            var newTarget = FindObjectsOfType<Entity>().Where(x => x.gameObject != this.gameObject && x.gameObject != _player.gameObject).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
            if (newTarget != null) CurrentTarget = newTarget;
            else CurrentTarget = _player;
        }
        else CurrentTarget = _player;
    }
}
