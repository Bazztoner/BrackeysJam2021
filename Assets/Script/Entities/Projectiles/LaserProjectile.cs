using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class LaserProjectile : BaseProyectile
{
    public float timeOut;

    public override void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        base.SpawnProjectile(position, direction);
        Invoke("TimeoutDestroy", timeOut);
    }

    public override void SpawnProjectile(Vector3 position, Vector3 direction, Entity owner)
    {
        base.SpawnProjectile(position, direction, owner);
        Invoke("TimeoutDestroy", timeOut);
    }

    void TimeoutDestroy()
    {
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.LayerMatchesWith("Enemy", "Player") && Owner != null && collider.gameObject != Owner.gameObject)
        {
            var ent = collider.GetComponent<Entity>();
            ent.TakeDamage(damage);
            CancelInvoke();
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Enemy", "Player") && Owner != null && collision.gameObject != Owner.gameObject)
        {
            var ent = collision.gameObject.GetComponent<Entity>();
            ent.TakeDamage(damage);
            CancelInvoke();
            Destroy(gameObject);
        }
    }
}
