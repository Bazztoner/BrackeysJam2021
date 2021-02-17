using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Parasyte", menuName = "New Parasyte Seed")]
public class SO_TypeSeed_Parasite : SO_TypeSeed_Generic
{
    public Effect mindControl;

    public float baseMinControlDuration;

    public float rateMindcontrolIncrease;

    public Effect GetMindControl()
    {
        float _mindControlDuration = baseMinControlDuration;

        float _mindControlIncrease = rateMindcontrolIncrease;

        if (amount > 1)
        {
            for (int i = 1; i < amount; i++)
            {
                _mindControlIncrease = (_mindControlIncrease * rateMindcontrolIncrease);
                _mindControlDuration += (baseMinControlDuration * _mindControlIncrease);
            }
        }

        return new Effect(TypeOfEffect.MindControl, _mindControlDuration);
    }
}
