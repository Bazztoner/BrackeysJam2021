using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combos", menuName = "New Seed-combo")]
public class SO_SeedCombos : ScriptableObject
{
    GameObject proyectile;

    public Effect mutateBase = new Effect(TypeOfEffect.Mutate, 1);
    public Effect mutateBouncer = new Effect(TypeOfEffect.Mutate, 2);
    public Effect mutateSeeker = new Effect(TypeOfEffect.Mutate, 3);

    public Effect dot = new Effect(TypeOfEffect.DamageOverTime, 1f);

    public void Initialize(Proyectile _proyectile)
    {
        proyectile = _proyectile.gameObject;
    }

    public void ComboBaseExplosive()
    {
        GameObject newProyectile1 = Instantiate(proyectile, proyectile.transform.position, proyectile.transform.rotation);
        GameObject newProyectile2 = Instantiate(proyectile, proyectile.transform.position, proyectile.transform.rotation);

        Vector3 dir1 = newProyectile1.transform.up + newProyectile1.transform.right;
        Vector3 dir2 = newProyectile1.transform.up - newProyectile1.transform.right;

        newProyectile1.GetComponent<Proyectile>().SpawnProjectile(proyectile.gameObject.transform.position, dir1, proyectile.GetComponent<Proyectile>().Owner);
        newProyectile2.GetComponent<Proyectile>().SpawnProjectile(proyectile.gameObject.transform.position, dir2, proyectile.GetComponent<Proyectile>().Owner);
    }

    public void ComboRootBouncer()
    {
        GameObject newProyectile = Instantiate(new GameObject());
        newProyectile.AddComponent<Rigidbody2D>();

        Proyectile _proyectile = newProyectile.AddComponent<Proyectile>();

        _proyectile.DefineCombo(SeedTypes.Bouncer, 1);
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
