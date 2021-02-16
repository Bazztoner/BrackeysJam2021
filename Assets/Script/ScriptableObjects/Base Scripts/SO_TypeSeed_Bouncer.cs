using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bauncer", menuName = "New Bauncer Seed")]
public class SO_TypeSeed_Bouncer : SO_TypeSeed_Generic
{
    int bounces;

    public override void Define(int amount, Transform objPos)
    {
        base.Define(amount, objPos);

        bounces = Mathf.RoundToInt(parameter);
    }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }

    public void Bounce()
    {
        bounces--;
    }
}
