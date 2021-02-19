using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniProyectile : MonoBehaviour
{
    Rigidbody2D rb;

    Transform destination;

    Vector3 _target =  Vector3.zero;

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

    public void SetDestination(Vector3 _destination)
    {
        _target = _destination;
    }

    private void LateUpdate()
    {
        if (destination != null) _target = destination.position;
        
        dir = _target - transform.position;

        transform.up = dir;

        rb.velocity = transform.up * speed;
    }
}
