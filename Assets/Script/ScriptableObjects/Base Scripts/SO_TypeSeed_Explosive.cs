using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosive", menuName = "New Explosive Seed")]
public class SO_TypeSeed_Explosive : SO_TypeSeed_Generic
{
    public float area;

    public float variationArea;

    public float amountKnockBack;

    public float variationKnockBack;

    public void AOE(int amount, Transform tfmProyectil)
    {
        float _area = area;
        float _variation = variationArea;

        for (int i = 1; i < amount; i++)
        {
            _variation = (_variation * variationArea);
            _area += (area * _variation);
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(tfmProyectil.position, _area);

        if (targets.Length >= 1)
        {
            float _amountKnockBack = amountKnockBack;
            float _variationKnockBack = variationKnockBack;

            for (int i = 1; i < amount; i++)
            {
                _variationKnockBack = (_variationKnockBack * variationKnockBack);
                _amountKnockBack += (amountKnockBack * _variationKnockBack);
            }

            foreach (var target in targets)
            {
                EnemyBase enemy = target.GetComponent<EnemyBase>() ? target.GetComponent<EnemyBase>() : null;

                if (enemy != null)
                {
                    enemy.RecieveEffect(new Effect(TypeOfEffect.KnockBack, _amountKnockBack, tfmProyectil.position));
                }
            }
        }
    }
}
