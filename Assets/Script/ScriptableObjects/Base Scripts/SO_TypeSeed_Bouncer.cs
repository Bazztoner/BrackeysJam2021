using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bauncer", menuName = "New Bauncer Seed")]
public class SO_TypeSeed_Bouncer : SO_TypeSeed_Generic
{
    public int bounces;

    public float bounceIncreaseRate;

    public int GetBounces(int amount)
    {
        float _bounces = bounces;
        float _bounceIncrease = bounceIncreaseRate;

        for (int i = 1; i < amount; i++)
        {
            _bounceIncrease = (_bounceIncrease * bounceIncreaseRate);
            _bounces += _bounceIncrease;
        }

        return Mathf.RoundToInt(_bounces);
    }

    public Vector3 Bounce(Transform tfmProyectil)
    {
        Vector3 dir = -tfmProyectil.right;


        return dir;
    }
}
