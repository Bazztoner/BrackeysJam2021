using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rooter", menuName = "New Rooter Seed")]
public class SO_TypeSeed_Rooter : SO_TypeSeed_Generic
{
    public Effect stun;

    public override void Define(int amount, Proyectile _proyectile)
    {
        base.Define(amount, _proyectile);

        stun = new Effect(TypeOfEffect.Stun, parameter);
        specialEffect = stun;
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
