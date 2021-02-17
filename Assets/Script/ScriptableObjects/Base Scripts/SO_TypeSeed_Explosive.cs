using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosive", menuName = "New Explosive Seed")]
public class SO_TypeSeed_Explosive : SO_TypeSeed_Generic
{
    public float area;

    public float variationArea;

    public Effect damageOverTime;

    public override void Define(int amount, Proyectile _proyectile)
    {
        proyectile = _proyectile;
        tfmProyectil = proyectile.gameObject.transform;

        for (int i = 0; i < amount; i++)
        {
            float _parameter = parameter;
            float _variationArea = variationArea;

            parameter += (_parameter * (i * variationRate));
            area += (_variationArea * (i * variationArea));
        }
    }

    public override void Create()
    {
        throw new System.NotImplementedException();
    }

    public override void Traverse()
    {
        throw new System.NotImplementedException();
    }

    public override void Impact()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(tfmProyectil.position, area);

        foreach (var target in targets)
        {
            EnemyBase enemy = target.GetComponent<EnemyBase>() ? target.GetComponent<EnemyBase>() : null;

            if (enemy != null)
            {
                damageOverTime = new Effect(TypeOfEffect.Damage, parameter);
                enemy.RecieveEffect(damageOverTime);
            }
        }
    }
}
