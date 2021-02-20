using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMissile : BaseProyectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{gameObject.name} collided into {collision.gameObject.name}.");

        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else if (collision.gameObject.LayerMatchesWith("Enemy") && collision.gameObject != Owner.gameObject)
        {
            collision.GetComponent<EnemyBase>().RecieveEffect(new Effect(TypeOfEffect.Damage, damage));
        }

        base.OnTriggerEnter2D(collision);
    }
}
