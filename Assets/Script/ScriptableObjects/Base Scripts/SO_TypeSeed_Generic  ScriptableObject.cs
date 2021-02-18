using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_TypeSeed_Generic : ScriptableObject
{
    [Tooltip("Plant spawned from this seed.")]
    [SerializeField]
    GameObject plant;

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

    public GameObject GetSprout()
    {
        return plant;
    }
}

public class Effect
{
    public TypeOfEffect type;

    public float modifier1, modifier2;

    public Vector3 dir;

    public Effect(TypeOfEffect _effect, float _mod1)
    {
        type = _effect;

        modifier1 = _mod1;
    }

    public Effect(TypeOfEffect _effect, float _mod1, Vector3 _dir)
    {
        type = _effect;

        modifier1 = _mod1;

        dir = _dir;
    }

    public Effect(TypeOfEffect _effect, float _mod1, float _mod2)
    {
        type = _effect;

        modifier1 = _mod1;

        modifier2 = _mod2;
    }
}

public enum TypeOfEffect
{
    Damage,
    KnockBack,
    Stun,
    DamageOverTime,
    MindControl,
    Mutate,
    TickBoom,
    Masochism
}