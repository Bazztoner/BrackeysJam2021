using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_TypeSeed_Generic : ScriptableObject
{
    [Tooltip("Type-seed name.")]
    [SerializeField]
    new string name;

    [Tooltip("Type-seed in-game narrative description.")]
    [SerializeField]
    [TextArea]
    string description;

    [Tooltip("Type-seed proyectile form base speed.")]
    [SerializeField]
    float speed;

    [Tooltip("Type-seed proyectile form life-span.")]
    [SerializeField]
    float life;

    [Tooltip("Type-seed proyectile form base damage.")]
    [SerializeField]
    float damage;

    [Tooltip("Sprite for the proyectile.")]
    [SerializeField]
    Sprite sprSeed;
}
