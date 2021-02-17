using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rooter", menuName = "New Rooter Seed")]
public class SO_TypeSeed_Rooter : SO_TypeSeed_Generic
{
    public float tickStun;

    public float stunChangeRate;

    public Effect GetStun()
    {
        float _tickStun = tickStun;
        float _stunChangeRate = stunChangeRate;

        for (int i = 1; i < amount; i++)
        {
            _stunChangeRate = (_stunChangeRate * stunChangeRate);
            _tickStun += _stunChangeRate;
        }

        return new Effect(TypeOfEffect.Stun, _tickStun);
    }
}
