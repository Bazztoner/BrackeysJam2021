using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bauncer", menuName = "New Bauncer Seed")]
public class SO_TypeSeed_Bouncer : SO_TypeSeed_Generic
{
    int bounces;

    public override void Define(int amount, Proyectile _proyectile)
    {
        base.Define(amount, _proyectile);

        bounces = Mathf.RoundToInt(parameter);
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
        if (bounces > 0)
        {
            Vector3 rotInit = tfmProyectil.up;
            Vector3 rotObjective = tfmProyectil.right;

            Vector3 rotEnd = rotObjective - rotInit;

            tfmProyectil.rotation = Quaternion.LookRotation(Vector3.zero, rotEnd);

            bounces--;
        }
    }
}
