using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;

/// <summary>
/// A.K.A MILF
/// </summary>
public class MotherShip : EnemyBase
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
    bool _hoverLeft;

    //Attack
    public float attackCooldown;
    float _currentAttackCooldown;
    public float shipSpeedMultiplier;

    EventFSM<Inputs> _stateMachine;
    public enum Inputs { EnemyFound, StateEnd, Die };

    public KamikazeShip kamikazePrefab;

    Entity _currentTarget;

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
        if (muzzle == null) muzzle = transform.Find("Muzzle");
        CurrentTarget = _player;
        InitFsm();
    }

    void InitFsm()
    {
        //-----------------------------------------STATE CREATE-------------------------------------------//
        var idle = new State<Inputs>("Idle");
        var engage = new State<Inputs>("Engage");
        var death = new State<Inputs>("Death");

        StateConfigurer.Create(idle)
            .SetTransition(Inputs.EnemyFound, engage)
            .SetTransition(Inputs.Die, death)
            .Done();

        StateConfigurer.Create(engage)
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

        };

        engage.OnUpdate += () =>
        {
            if (_currentAttackCooldown >= attackCooldown)
            {
                Shoot();
                _currentAttackCooldown = 0;
            }
            else
            {
                _currentAttackCooldown += Time.deltaTime;
            }
        };

        engage.OnFixedUpdate += () =>
        {
            var movMultiplicator = _hoverLeft ? -1 : 1;
            _rb.MovePosition(_rb.position + Vector2.right * (movementSpeed * movMultiplicator * _movementModifier) * Time.fixedDeltaTime);
            _currentDirectionTime += Time.fixedDeltaTime;
            if (_currentDirectionTime >= directionMovementCheckTime)
            {
                _hoverLeft = !_hoverLeft;
                _currentDirectionTime = 0f;
            }
        };

        engage.OnExit += x =>
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
    }
    public void OnTakeDamage()
    {
        if (CurrentHP <= 0)
        {
            Die();
        }
        else ProcessInput(Inputs.EnemyFound);
    }

    public override void Die()
    {
        ProcessInput(Inputs.Die);
        base.Die();
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
            if (Vector2.Distance(CurrentTarget.transform.position, transform.position) <= distanceToEngage)
            {
                ProcessInput(Inputs.EnemyFound);
            }
        }
    }

    protected override void Shoot()
    {
       var newShip = GameObject.Instantiate(kamikazePrefab, muzzle.transform.position, Quaternion.identity);

        newShip.SpawnByMother(muzzle.transform.position, shipSpeedMultiplier, CurrentTarget);
    }

    protected override void StunHandler(bool state)
    {
        _movementModifier = state ? 0 : 1;
    }

    protected override void MindControlHandler(bool state)
    {
        if (state)
        {
            var newTarget = FindObjectsOfType<Entity>().Where(x => x.gameObject != this.gameObject && x.gameObject != _player.gameObject).OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).FirstOrDefault();
            if (newTarget != null) CurrentTarget = newTarget;
            else CurrentTarget = _player;
        }
        else CurrentTarget = _player;
    }
}
