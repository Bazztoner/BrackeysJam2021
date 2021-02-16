using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Parasyte", menuName = "New Parasyte Seed")]
public class SO_TypeSeed_Parasyte : SO_TypeSeed_Generic
{
    public Effect mindControl;

    public override void Define(int amount, Proyectile _proyectile)
    {
        base.Define(amount, _proyectile);

        mindControl = new Effect(TypeOfEffect.MindControl, parameter);
        specialEffect = mindControl;
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
        throw new System.NotImplementedException();
    }
}
