using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingMissile : BaseProyectile
{
    protected Transform target;

    [SerializeField]
    [Range(0.01f, 0.99f)]
    protected float viration = .1f;

    public void DefineTarget(Transform _target)
    {
        target = _target;
    }

    protected override void FixedUpdate()
    {
        CorrectDirection();

        base.FixedUpdate();
    }

    protected void CorrectDirection()
    {
        Vector3 dirInit = transform.up;
        Vector3 dirEnd = target.position - transform.position;

        transform.up = Vector3.Lerp(dirInit, dirEnd, (viration * Time.fixedDeltaTime));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else if (collision.gameObject.LayerMatchesWith("Enemy"))
        {
            collision.GetComponent<EnemyBase>().RecieveEffect(new Effect(TypeOfEffect.Damage, damage));
        }

        base.OnTriggerEnter2D(collision);
    }
}
