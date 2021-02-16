using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;

public class ScoutShip : EnemyBase
{
    //FSM
    //engage
    public float distanceToEngage;
    public float directionMovementCheckTime;
    float _currentDirectionTime;

    public float hoverSpeed;
    bool _hoverLeft;

    //Attack
    public float attackCooldown;
    float _currentAttackCooldown;

    [Header("Percentage to attack (percentage per second / time.deltaTime")]
    [Range(0, 50)]
    public int attackPerc;

    public float attackRange;

    bool _isAttacking;

    public float attackChannelTime, dashDuration, dashForce, afterAttackStun;

    private EventFSM<Inputs> _stateMachine;
    public enum Inputs { EnemyFound, EnemyInAttackRange, StateEnd, Die };

    public string GetCurrentState()
    {
        return _stateMachine.Current.Name;
    }

    protected override void Start()
    {
        base.Start();
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
            var dir = _player.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;
        };

        engage.OnFixedUpdate += () =>
        {
            var dir = _player.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;

            //move to player if distance > attackRange/2
            if (Vector3.Distance(_player.transform.position, transform.position) >= attackRange / 2)
            {
                _rb.MovePosition(_rb.position + (Vector2)dir * movementSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //else move left or right
                var movMultiplicator = _hoverLeft ? -1 : 1;
                _rb.MovePosition(_rb.position + (Vector2)transform.right * (hoverSpeed * movMultiplicator) * Time.fixedDeltaTime);
                _currentDirectionTime += Time.fixedDeltaTime;
                if (_currentDirectionTime >= directionMovementCheckTime)
                {
                    _hoverLeft = UnityEngine.Random.Range(0, 100) >= 50;
                    _currentDirectionTime = 0f;
                }
            }

        };

        engage.OnExit += x =>
        {

        };

        //attack
        attack.OnEnter += x =>
        {
            StartCoroutine(AttackBehaviourHandler());
        };

        attack.OnExit += x =>
        {
            StartCoroutine(AttackCooldownHandler());
        };

        //death
        death.OnEnter += x =>
        {

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
            if (Vector3.Distance(_player.transform.position, transform.position) <= distanceToEngage)
            {
                ProcessInput(Inputs.EnemyFound);
            }
        }

        if (Vector3.Distance(_player.transform.position, transform.position) <= attackRange && _currentAttackCooldown <= 0 && !_isAttacking)
        {
            if (Random.Range(0, 100) >= attackPerc)
            {
                ProcessInput(Inputs.EnemyInAttackRange);
            }
        }
    }

    IEnumerator AttackBehaviourHandler()
    {
        _isAttacking = true;

        //channel time
        //do particles
        var currentChannelTime = 0f;
        Vector3 dir;

        while (currentChannelTime <= attackChannelTime)
        {
            dir = _player.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;

            yield return new WaitForEndOfFrame();

            currentChannelTime += Time.deltaTime;
        }

        //dash time
        var dashDirection = UnityEngine.Random.Range(0, 100) >= 50 ? 1 : -1;

        _rb.AddForce(transform.right * (dashForce * dashDirection), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);

        //Reset after dash - this may fuck up the knockback from the base seed but who the fucking fuck cares
        _rb.velocity = Vector2.zero;

        //shoot shit
        dir = _player.transform.position - transform.position;
        transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;

        //do shooting

        //get mini-stunned
        yield return new WaitForSeconds(afterAttackStun);


        _isAttacking = false;
        AttackEnd();
    }

    IEnumerator AttackCooldownHandler()
    {
        _currentAttackCooldown = attackCooldown;

        while (_currentAttackCooldown > 0)
        {
            yield return new WaitForEndOfFrame();

            _currentAttackCooldown -= Time.deltaTime;
        }

        _currentAttackCooldown = 0;
    }

}
