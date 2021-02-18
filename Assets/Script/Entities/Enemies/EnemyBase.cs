﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FSM;

public abstract class EnemyBase : Entity
{
    public float maxHP, movementSpeed;
    protected float _currentHP;

    protected Rigidbody2D _rb;
    public BaseProjectile projectile;
    protected PlayerController _player;

    [Header("Point where the projectiles spawn")]
    public Transform muzzle;

    protected bool _isStunned, _isMindControlled, _isTicking;

<<<<<<< HEAD
    List<SeedTypes> inyectedSeeds = new List<SeedTypes>();
=======
    protected List<SeedTypes> _inyectedSeeds;
>>>>>>> 0d0d2888307858810208ec433f1dbb7461c77113

    public float CurrentHP
    {
        get => _currentHP;
        protected set
        {
            _currentHP = value;
            if (_currentHP > maxHP) _currentHP = maxHP;
            else if (_currentHP < 0) _currentHP = 0;
        }
    }

    protected virtual void Start()
    {
        CurrentHP = maxHP;
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindObjectOfType<PlayerController>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected abstract void Shoot();

    public override void TakeDamage(float dmg)
    {
        CurrentHP -= dmg;
    }

    IEnumerator Stunned(float tick)
    {
        _isStunned = true;

        yield return new WaitForSeconds(tick);

        _isStunned = false;
    }

    IEnumerator MindControlled(float tick)
    {
        _isMindControlled = true;

        yield return new WaitForSeconds(tick);

        _isMindControlled = false;
    }

    IEnumerator DoT(float tick, float damage)
    {
        float _tick = tick;

        while (_tick > 0f)
        {
            TakeDamage(damage);

            yield return new WaitForEndOfFrame();

            _tick -= Time.fixedDeltaTime;
        }
    }

    public virtual void Explode()
    {

    }

    IEnumerator TickBoom(float tick)
    {
        float _tick = tick;

        _isTicking = true;

        while (_tick > 0f)
        {
            yield return new WaitForEndOfFrame();

            _tick -= Time.fixedDeltaTime;
        }

        Explode();
    }

    public virtual void RecieveEffect(Effect _effect)
    {
        switch (_effect.type)
        {
            case TypeOfEffect.Damage:
                TakeDamage(_effect.modifier1);
                break;
            case TypeOfEffect.KnockBack:
                Vector3 dir = transform.position - _effect.dir;

                Vector2 _dir = new Vector2(dir.x, dir.y) * _effect.modifier1;

                _rb.AddForce(_dir, ForceMode2D.Impulse);
                break;
            case TypeOfEffect.Stun:
                StartCoroutine(Stunned(_effect.modifier1));
                break;
            case TypeOfEffect.DamageOverTime:
                StartCoroutine(DoT(_effect.modifier1, _effect.modifier2));
                break;
            case TypeOfEffect.MindControl:
                StartCoroutine(MindControlled(_effect.modifier1));
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
