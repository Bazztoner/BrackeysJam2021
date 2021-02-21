using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;

public class KamikazeShip : EnemyBase
{
    //FSM
    /// <summary>
    /// 0 if rooted, 1 if normal
    /// </summary>
    float _movementModifier = 1f;
    //engage
    public float distanceToEngage;
    public float explosionDistance;
    public float explosionRadius, explosionDamage;

    private EventFSM<Inputs> _stateMachine;
    public enum Inputs { EnemyFound, StateEnd, Die };

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

    public void SpawnByMother(Vector3 position, float speedMultiplier, Entity target)
    {
        Start();
        transform.position = position;
        movementSpeed *= speedMultiplier;
        CurrentTarget = target != null ? target : _player;
        if (GetCurrentState() == "Idle")
        {
            ProcessInput(Inputs.EnemyFound);
        }
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
            var dir = CurrentTarget.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;
        };

        engage.OnFixedUpdate += () =>
        {
            var dir = CurrentTarget.transform.position - transform.position;
            transform.up = new Vector3(dir.x, dir.y, transform.up.z).normalized;
            _rb.MovePosition(_rb.position + (Vector2)dir * (movementSpeed * _movementModifier) * Time.fixedDeltaTime);
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
            Explode();
        }
        else ProcessInput(Inputs.EnemyFound);
    }

    public override void Explode()
    {
        base.Explode();
        var ents = Physics2D.OverlapCircleAll(transform.position, explosionRadius)
            .Select(x => x.GetComponent<Entity>())
            .Where(x => x != null && x.gameObject != gameObject);

        foreach (var item in ents)
        {
            item.TakeDamage(explosionDamage);
        }
    }

    public override void Die()
    {
        base.Die();
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
            if (Vector2.Distance(CurrentTarget.transform.position, transform.position) <= distanceToEngage)
            {
                ProcessInput(Inputs.EnemyFound);
            }
        }

        if (Vector2.Distance(CurrentTarget.transform.position, transform.position) <= explosionDistance)
        {
            Explode();
        }
    }

    protected override void Shoot()
    {
        //Does not apply
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
