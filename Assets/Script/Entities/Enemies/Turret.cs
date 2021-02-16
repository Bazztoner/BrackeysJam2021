using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;

public class Turret : EnemyBase
{
    //Force pos
    public Vector3 startingPosition;

    //FSM

    //Idle
    public float rotationSpeed;

    //Attack
    [Header("Cooldown between shots")]
    public float attackCooldown;
    float _currentAttackCooldown;

    public float attackRange;

    private EventFSM<Inputs> _stateMachine;
    public enum Inputs { EnemyInAttackRange, StateEnd, Die };

    public string GetCurrentState()
    {
        return _stateMachine.Current.Name;
    }

    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
        InitFsm();
    }

    void InitFsm()
    {
        //-----------------------------------------STATE CREATE-------------------------------------------//
        var idle = new State<Inputs>("Idle");
        var attack = new State<Inputs>("Attack");
        var death = new State<Inputs>("Death");

        StateConfigurer.Create(idle)
            .SetTransition(Inputs.EnemyInAttackRange, attack)
            .SetTransition(Inputs.Die, death)
            .Done();

        StateConfigurer.Create(attack)
           .SetTransition(Inputs.StateEnd, idle)
           .SetTransition(Inputs.Die, death)
           .Done();

        StateConfigurer.Create(death).Done();

        //-----------------------------------------STATE SET-------------------------------------------//

        //idle

        idle.OnUpdate += () =>
        {
            transform.Rotate(transform.forward, rotationSpeed * Time.deltaTime);
        };

        //attack
        attack.OnEnter += x =>
        {
            _currentAttackCooldown = 0;
            var dir = _player.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;
        };

        attack.OnUpdate += () =>
        {
            var dir = _player.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;

            if (_currentAttackCooldown >= attackCooldown)
            {
                //shoot dir transform.up
                Shoot();
                _currentAttackCooldown = 0;
            }
            else _currentAttackCooldown += Time.deltaTime;

        };

        //death
        death.OnEnter += x =>
        {
            //BOOM!
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
        _rb.velocity = Vector2.zero;
    }

    protected override void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
        _rb.velocity = Vector2.zero;
        _rb.MovePosition(startingPosition);
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
    }

    public void Die()
    {
        ProcessInput(Inputs.Die);
    }
    protected override void Shoot()
    {
        var instancedProjectile = GameObject.Instantiate(projectile, muzzle.transform.position, Quaternion.identity);
        instancedProjectile.SpawnProjectile(muzzle.transform.position, transform.up, this);
    }

    //Sensor checking
    void CheckSensors()
    {
        if (GetCurrentState() == "Death") return;

        if (Vector3.Distance(_player.transform.position, transform.position) <= attackRange)
        {
            ProcessInput(Inputs.EnemyInAttackRange);
        }
        else ProcessInput(Inputs.StateEnd);
    }
}
