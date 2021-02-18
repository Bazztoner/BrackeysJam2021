using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniProjectile : MonoBehaviour
{
    Rigidbody2D rb;

    Transform destination;

    Vector3 dir;

    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDestination(Transform _destination)
    {
        destination = _destination;
    }

    private void LateUpdate()
    {
        if (destination != null) dir = destination.position - transform.position;

        transform.up = dir;

        rb.velocity = transform.up * speed;
    }
}
