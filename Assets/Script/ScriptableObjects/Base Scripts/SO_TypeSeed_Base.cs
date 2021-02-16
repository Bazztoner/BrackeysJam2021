using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base", menuName = "New Base Seed")]
public class SO_TypeSeed_Base : SO_TypeSeed_Generic
{
    public Effect knockBack;

    public override void Define(int amount, Proyectile _proyectile)
    {
        base.Define(amount, _proyectile);

        knockBack = new Effect(TypeOfEffect.KnocBack, parameter);
        specialEffect = knockBack;
    }

    public override void Create()
    {
        throw new System.NotImplementedException();
    }

    public override void Impact()
    {
        throw new System.NotImplementedException();
    }

    public override void Traverse()
    {
        throw new System.NotImplementedException();
    }
}
