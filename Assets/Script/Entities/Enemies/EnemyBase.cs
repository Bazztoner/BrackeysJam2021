using System.Collections;
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

    protected float CurrentHP
    {
        get => _currentHP;
        set
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

    public void RecieveEffect(Effect _effect)
    {
        switch (_effect.type)
        {
            case TypeOfEffect.Damage:
                TakeDamage(_effect.modifier1);
                break;
            case TypeOfEffect.KnocBack:

                break;
            case TypeOfEffect.Stun:

                break;
            case TypeOfEffect.DamageOverTime:

                break;
            case TypeOfEffect.MindControl:

                break;
            default:
                break;
        }
    }
}
