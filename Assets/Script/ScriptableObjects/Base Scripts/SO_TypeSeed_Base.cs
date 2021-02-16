using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base", menuName = "New Base Seed")]
public class SO_TypeSeed_Base : SO_TypeSeed_Generic
{
    public Effect knockBack;

    public override void Define(int amount, Transform objPos)
    {
        base.Define(amount, objPos);

        knockBack = new Effect(TypeOfEffect.KnocBack, parameter);
        specialEffect = knockBack;
    }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }
}
