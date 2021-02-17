using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_TypeSeed_Generic : ScriptableObject
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

    internal Proyectile proyectile;

    internal Transform tfmProyectil;

    public Effect specialEffect;

    public float parameter;

    public float variationRate;

    public virtual void Define(int amount, Proyectile _proyectile)
    {
        proyectile = _proyectile;
        tfmProyectil = proyectile.gameObject.transform;

        float value = variationRate;

        for (int i = 0; i < amount; i++)
        {
            float _parameter = parameter;

            parameter += (_parameter * (i * value));
        }
    }

    public abstract void Create();
    public abstract void Traverse();
    public abstract void Impact();
}

public class Effect
{
    public TypeOfEffect type;

    public float modifier1, modifier2;

    public Effect(TypeOfEffect _effect, float _mod1)
    {
        type = _effect;

        modifier1 = _mod1;
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
    MindControl
}
