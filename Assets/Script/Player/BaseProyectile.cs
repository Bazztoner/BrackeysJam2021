using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProyectile : MonoBehaviour
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
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), owner.GetComponent<Collider2D>());
    }

    protected virtual void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
        //_rb.velocity = ((Vector2)transform.up * speed * Time.fixedDeltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
