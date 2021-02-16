using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    protected Rigidbody2D _rb;

    public float damage;

    public float speed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.up = direction;
        //owner = entityOwner
    }

    void FixedUpdate()
    {
        _rb.MovePosition(transform.up * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} collided into {collision.gameObject.name}");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{gameObject.name} collided into {collision.gameObject.name}");
        Destroy(gameObject);
    }
}
