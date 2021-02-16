using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosive", menuName = "New Explosive Seed")]
public class SO_TypeSeed_Explosive : SO_TypeSeed_Generic
{
    public float parameter2;

    public float variationRate2;

    public float area;

    public float variationArea;

    public Effect damageOverTime;

    public override void Define(int amount, Transform objPos)
    {
        pos = objPos;

        for (int i = 0; i < amount; i++)
        {
            float _parameter = parameter;
            float _parameter2 = parameter2;
            float _variationArea = variationArea;

            parameter += (_parameter * (i * variationRate));
            parameter2 += (_parameter2 * (i * variationRate2));
            area += (_variationArea * (i * variationArea));
        }

        damageOverTime = new Effect(TypeOfEffect.KnocBack, parameter, parameter2);
        specialEffect = damageOverTime;
    }

    public override void Behaviour()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos.position, area);

        EnemyBase enemy = null;

        foreach (var target in targets)
        {
            if (target.GetComponent<EnemyBase>())
            {
            }
        }
    }
}
