using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Parasyte", menuName = "New Parasyte Seed")]
public class SO_TypeSeed_Parasyte : SO_TypeSeed_Generic
{
    public Effect mindControl;

    public override void Define(int amount, Transform objPos)
    {
        base.Define(amount, objPos);

        mindControl = new Effect(TypeOfEffect.MindControl, parameter);
        specialEffect = mindControl;
    }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }
}
