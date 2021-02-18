using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCollectible : MonoBehaviour
{
    [SerializeField]
    protected float value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Enemy") || collision.gameObject.LayerMatchesWith("Player"))
        {
            collision.gameObject.GetComponent<Entity>().TakeHeal(value);

            Destroy(gameObject);
        }
    }
}
