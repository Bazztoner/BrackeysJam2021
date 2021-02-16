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
    public float speed;

    [Tooltip("Type-seed proyectile form base damage.")]
    public float damage;

    [Tooltip("Sprite for the proyectile.")]
    [SerializeField]
    Sprite sprSeed;

    public virtual void Define()
    {

    }

    public virtual float DefineDamage(int amount)
    {
        float baseDamage = damage;

        for (int i = 0; i < amount; i++)
        {
            baseDamage += (damage - i);
        }

        return baseDamage;
    }
}
