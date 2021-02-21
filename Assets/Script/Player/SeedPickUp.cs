﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SeedPickUp : MonoBehaviour
{
    public UnityEvent onPickUp;

    public SeedTypes type;

    public int GetTypeSeed()
    {
        return (int)type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
            onPickUp.Invoke();
    }
}