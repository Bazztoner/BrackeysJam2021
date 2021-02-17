using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base", menuName = "New Base Seed")]
public class SO_TypeSeed_Base : SO_TypeSeed_Generic
{
    public float amountKnockBack;

    public float increaseRate;

    public Effect GetKnockBack()
    {
        float _amountKnockBack = amountKnockBack;
        float _variationKnockBack = increaseRate;

        for (int i = 1; i < amount; i++)
        {
            _variationKnockBack = (_variationKnockBack * increaseRate);
            _amountKnockBack += (amountKnockBack * _variationKnockBack);
        }

        return new Effect(TypeOfEffect.KnockBack, _amountKnockBack);
    }
}
