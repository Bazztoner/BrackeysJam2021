using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    Rigidbody2D _rb;

    public float damage;

    public float speed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rb.MovePosition(transform.up * speed * Time.deltaTime);
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
