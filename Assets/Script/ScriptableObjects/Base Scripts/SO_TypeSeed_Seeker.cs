using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seeker", menuName = "New Seeker Seed")]
public class SO_TypeSeed_Seeker : SO_TypeSeed_Generic
{
    float precision;

    public override void Define(int amount, Transform objPos)
    {
        base.Define(amount, objPos);

        precision = parameter;
    }

    public override void Behaviour()
    {
        throw new System.NotImplementedException();
    }

    public void Seek()
    {

    }
}
