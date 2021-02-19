using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;
using System;

public class HiveBoss : EnemyBase
{
    //FSM
    //engage
    public float distanceToEngage;
    public float directionMovementCheckTime;
    float _currentDirectionTime;

    bool _hoverLeft;

    //Attack
    /// <summary>
    /// Cooldown after attacking; when in 0, the enemy do one of four attacks
    /// </summary>
    public float attackCooldown;
    float _currentAttackCooldown;
    bool _isAttacking;

    //Attack 1
    //Random hiveChild attack
    HiveChild[] _children;

    //Attack 2
    //Center big projectile
    public BaseProyectile projectile;

    //Attack 3
    //Left cannon rapidfire machinegun
    public float leftCannonAttackCooldown;
    public HiveCannon leftCannon;
    public float leftCannonAttackDuration;

    //Attack 4
    //Right cannon slowfire three-burst shotgun
    public float rightCannonAttackCooldown;
    public HiveCannon rightCannon;
    public float rightCannonAttackDuration;

    Collider2D[] _allColls;

    EventFSM<Inputs> _stateMachine;
    public enum Inputs { EnemyFound, EnemyInAttackRange, StateEnd, Die };

    public string GetCurrentState()
    {
        return _stateMachine.Current.Name;
    }

    protected override void Start()
    {
        base.Start();
        if (muzzle == null) muzzle = transform.Find("Muzzle");
        CurrentHP = maxHP;
        _children = GetComponentsInChildren<HiveChild>(true);
        _allColls = GetComponentsInChildren<Collider2D>(true);
        InitFsm();
    }

    void InitFsm()
    {
        //-----------------------------------------STATE CREATE-------------------------------------------//
        var idle = new State<Inputs>("Idle");
        var attack = new State<Inputs>("Attack");
        var death = new State<Inputs>("Death");

        StateConfigurer.Create(idle)
            .SetTransition(Inputs.EnemyFound, attack)
            .SetTransition(Inputs.Die, death)
            .Done();

        StateConfigurer.Create(attack)
           .SetTransition(Inputs.Die, death)
           .Done();

        StateConfigurer.Create(death).Done();

        //-----------------------------------------STATE SET-------------------------------------------//

        //idle
        idle.OnEnter += x =>
        {

        };

        //attack
        attack.OnEnter += x =>
        {

        };

        attack.OnFixedUpdate += () =>
        {
            var movMultiplicator = _hoverLeft ? -1 : 1;
            _rb.MovePosition(_rb.position + Vector2.right * (movementSpeed * movMultiplicator) * Time.fixedDeltaTime);
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
            else if (!_isAttacking)
            {
                _currentAttackCooldown += Time.deltaTime;
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
    }

    public void OnTakeDamage()
    {
        if (CurrentHP <= 0)
        {
            Die();
        }
        else ProcessInput(Inputs.EnemyFound);
    }

    public void ClearDestroyedShips(HiveChild clear)
    {
        _children = _children.Where(x => x.gameObject != clear.gameObject && x != null).ToArray();
    }

    public void Die()
    {
        ProcessInput(Inputs.Die);
        StopAllCoroutines();
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
            if (Vector3.Distance(_player.transform.position, transform.position) <= distanceToEngage)
            {
                ProcessInput(Inputs.EnemyFound);
            }
        }
    }

    protected override void Shoot()
    {
        List<Action> attacks = new List<Action>();
        if (Attack1Enabled()) attacks.Add(() => Attack1Handler());
        if (Attack2Enabled()) attacks.Add(() => Attack2Handler());
        if (Attack3Enabled()) attacks.Add(() => Attack3Handler());
        if (Attack4Enabled()) attacks.Add(() => Attack4Handler());

        var rndAttack = UnityEngine.Random.Range(0, attacks.Count());

        attacks[rndAttack]();

        //_isAttacking = true;
    }

    void ManageColliders()
    {
        _allColls = _allColls.Where(x => x != null && x.gameObject != null).ToArray();
    }

    public bool Attack1Enabled() { return _children.Any(); }
    public void Attack1Handler()
    {
        if (!_children.Any()) return;

        var rndAttack = UnityEngine.Random.Range(0, _children.Length);
        ManageColliders();
        _children[rndAttack].Shoot(_allColls);
    }

    public bool Attack2Enabled() { return true; }
    public void Attack2Handler()
    {
        ManageColliders();

        var instancedProjectile = GameObject.Instantiate(projectile, muzzle.transform.position, Quaternion.identity);
        var col = instancedProjectile.GetComponentInChildren<Collider2D>();
        foreach (var item in _allColls)
        {
            Physics2D.IgnoreCollision(col, item);
        }
        instancedProjectile.SpawnProjectile(muzzle.transform.position, Vector2.down, this);
    }

    public bool Attack3Enabled() { return leftCannon != null; }
    public void Attack3Handler()
    {
        StartCoroutine(Attack3Coroutine());
    }

    IEnumerator Attack3Coroutine()
    {
        _isAttacking = true;
        float _duration = 0f;
        float _attackCooldown = 0f;
        while (_duration <= leftCannonAttackDuration)
        {
            if (leftCannon == null)
            {
                _isAttacking = false;
                yield break;
            }

            if (_attackCooldown >= leftCannonAttackCooldown)
            {
                ManageColliders();
                leftCannon.Shoot(Vector2.down, _allColls);
                _attackCooldown = 0;
            }
            else
            {
                _attackCooldown += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();

            if (leftCannon == null)
            {
                _isAttacking = false;
                yield break;
            }

            _duration += Time.deltaTime;
        }
        _isAttacking = false;
    }

    public bool Attack4Enabled() { return rightCannon != null; }
    public void Attack4Handler()
    {
        StartCoroutine(Attack4Coroutine());
    }

    IEnumerator Attack4Coroutine()
    {
        _isAttacking = true;
        float _duration = 0f;
        float _attackCooldown = 0f;
        while (_duration <= rightCannonAttackDuration)
        {
            if (rightCannon == null)
            {
                _isAttacking = false;
                yield break;
            }

            if (_attackCooldown >= rightCannonAttackCooldown)
            {
                ManageColliders();
                rightCannon.Shoot(Vector2.down.Vector2FromAngle(-30f) + Vector2.down, _allColls);
                rightCannon.Shoot(Vector2.down.Vector2FromAngle(-60f) + Vector2.down, _allColls);
                rightCannon.Shoot(Vector2.down, _allColls);
                rightCannon.Shoot(Vector2.down.Vector2FromAngle(30f) + Vector2.down, _allColls);
                rightCannon.Shoot(Vector2.down.Vector2FromAngle(60f) + Vector2.down, _allColls);
                _attackCooldown = 0;
            }
            else
            {
                _attackCooldown += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();

            if (rightCannon == null)
            {
                _isAttacking = false;
                yield break;
            }

            _duration += Time.deltaTime;
        }
        _isAttacking = false;
    }

    protected override void StunHandler(bool state)
    {
        //Inmune
    }

    protected override void MindControlHandler(bool state)
    {
        //Inmune
    }

    public override void RecieveEffect(Effect _effect)
    {
        switch (_effect.type)
        {
            case TypeOfEffect.Damage:
                TakeDamage(_effect.modifier1);
                fbMan.DirectionalDamage(_effect.dir);
                break;
            case TypeOfEffect.KnockBack:
                break;
            case TypeOfEffect.Stun:
                break;
            case TypeOfEffect.DamageOverTime:
                StartCoroutine(DoT(_effect.modifier1, _effect.modifier2));
                break;
            case TypeOfEffect.MindControl:
                break;
            case TypeOfEffect.Mutate:
                _inyectedSeeds.Add((SeedTypes)_effect.modifier1);
                break;
            case TypeOfEffect.TickBoom:
                StartCoroutine(TickBoom(_effect.modifier1));
                break;
            default:
                print("No effects received");
                break;
        }
    }
}