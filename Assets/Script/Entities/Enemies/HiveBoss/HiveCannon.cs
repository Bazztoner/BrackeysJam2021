using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HiveCannon : EnemyBase
{
    public HiveBoss bossParent;
    public BaseProyectile projectile;

    protected override void Start()
    {
        if (muzzle == null) muzzle = transform.Find("Muzzle");
        bossParent = GetComponentInParent<HiveBoss>();
        base.Start();
    }

    protected override void Update()
    {
        transform.up = Vector3.up;
    }

    public override void TakeDamage(float dmg)
    {
        CurrentHP -= dmg;
        //bossParent.TakeDamage(dmg);
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeHeal(float hp)
    {
        CurrentHP += hp;
        //bossParent.TakeHeal(hp);
    }

    public void Shoot(Vector2 dir, Collider2D[] cols)
    {
        var instancedProjectile = GameObject.Instantiate(projectile, muzzle.transform.position, Quaternion.identity);
        var col = instancedProjectile.GetComponentInChildren<Collider2D>();
        foreach (var item in cols)
        {
            Physics2D.IgnoreCollision(col, item);
        }
        instancedProjectile.SpawnProjectile(muzzle.transform.position, dir, this);
    }

    protected override void Shoot()
    {
        //USA LA OTRAAAAAAAAAAAAAAA
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