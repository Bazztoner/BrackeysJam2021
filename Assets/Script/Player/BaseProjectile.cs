using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    protected Rigidbody2D _rb;

    public float damage;

    public float speed;

    protected Entity _owner;

    public Entity Owner { get => _owner; protected set => _owner = value; }

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public virtual void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.up = direction;
    }

    public virtual void SpawnProjectile(Vector3 position, Vector3 direction, Entity owner)
    {
        transform.position = position;
        transform.up = direction;
        Owner = owner;
    }

    protected virtual void FixedUpdate()
    {
        _rb.MovePosition(transform.up * speed * Time.fixedDeltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} collided into {collision.gameObject.name}");
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{gameObject.name} collided into {collision.gameObject.name}");
        Destroy(gameObject);
    }
}
