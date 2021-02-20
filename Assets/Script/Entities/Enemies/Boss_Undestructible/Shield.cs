using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    protected float shieldDamage, knockback;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(shieldDamage);

            Vector3 knck = Vector3.Normalize(collision.transform.position - transform.position) * knockback;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knck, ForceMode2D.Impulse);
        }

        if (collision.gameObject.LayerMatchesWith("Projectile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
