using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combos", menuName = "New Seed-combo")]
public class SO_SeedCombos : ScriptableObject
{
    GameObject proyectile;

    public void Initialize(Proyectile _proyectile)
    {
        proyectile = _proyectile.gameObject;
    }

    public void ComboBaseRoot()
    {

    }

    public void ComboBaseExplosive()
    {

    }

    public void ComboBaseBouncer()
    {

    }

    public void ComboBaseSeeker()
    {

    }

    public void ComboBaseParasite()
    {

    }

    public void ComboRootExplosive()
    {

    }

    public void ComboRootBouncer()
    {

    }

    public void ComboRootSeeker()
    {

    }

    public void ComboRootParasite()
    {

    }

    public void ComboExplosiveBouncer()
    {

    }

    public void ComboExplosiveSeeker()
    {

    }

    public void ComboExplosiveParasite()
    {

    }

    public void ComboBouncerSeeker()
    {

    }

    public void ComboBouncerParasite()
    {

    }

    public void ComboSeekerParasite()
    {

    }
}
