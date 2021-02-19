using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMissile : BaseProyectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }

        base.OnTriggerEnter2D(collision);
    }
}
