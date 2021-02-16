using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rooter", menuName = "New Rooter Seed")]
public class SO_TypeSeed_Rooter : SO_TypeSeed_Generic
{
    public Effect stun;

    public override void Define(int amount, Transform objPos)
    {
        base.Define(amount, objPos);

        stun = new Effect(TypeOfEffect.Stun, parameter);
        specialEffect = stun;
    }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }
}
