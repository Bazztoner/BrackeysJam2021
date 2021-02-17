using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combos", menuName = "New Seed-combo")]
public class SO_SeedCombos : ScriptableObject
{
    GameObject proyectile;

    public GameObject miniProyectile;

    public void Initialize(Proyectile _proyectile)
    {
        proyectile = _proyectile.gameObject;
    }

    public void ComboBaseExplosive()
    {
        Queue<SeedTypes> _seed = new Queue<SeedTypes>();

        _seed.Enqueue(SeedTypes.Base);

        GameObject por1 = proyectile.GetComponent<Proyectile>().Owner.gameObject.GetComponent<ComboSystem>().DefineCombo(_seed).gameObject;
        GameObject por2 = proyectile.GetComponent<Proyectile>().Owner.gameObject.GetComponent<ComboSystem>().DefineCombo(_seed).gameObject;

        Vector3 dir1 = por1.transform.up + por1.transform.right;
        Vector3 dir2 = por2.transform.up - por2.transform.right;

        por1.GetComponent<Proyectile>().SpawnProjectile(proyectile.gameObject.transform.position, dir1, proyectile.GetComponent<Proyectile>().Owner);
        por2.GetComponent<Proyectile>().SpawnProjectile(proyectile.gameObject.transform.position, dir2, proyectile.GetComponent<Proyectile>().Owner);
    }

    public void ComboRootBouncer()
    {
        Queue<SeedTypes> _seed = new Queue<SeedTypes>();

        _seed.Enqueue(SeedTypes.Bouncer);

        GameObject pro = proyectile.GetComponent<Proyectile>().Owner.gameObject.GetComponent<ComboSystem>().DefineCombo(_seed).gameObject;

        Vector3 dir1 = pro.transform.up + pro.transform.right;

        pro.GetComponent<Proyectile>().SpawnProjectile(proyectile.gameObject.transform.position, dir1, proyectile.GetComponent<Proyectile>().Owner);
    }

    public void ComboRootSeeker()
    {
        Queue<SeedTypes> _seed = new Queue<SeedTypes>();

        _seed.Enqueue(SeedTypes.Seeker);

        GameObject pro = proyectile.GetComponent<Proyectile>().Owner.gameObject.GetComponent<ComboSystem>().DefineCombo(_seed).gameObject;

        Vector3 dir1 = pro.transform.up + pro.transform.right;

        pro.GetComponent<Proyectile>().SpawnProjectile(proyectile.gameObject.transform.position, dir1, proyectile.GetComponent<Proyectile>().Owner);
    }

    public void ComboExplosiveBouncer()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

        if (enemies.Length >= 1)
        {
            EnemyBase closest = enemies[0];

            if (enemies.Length > 1)
            {
                for (int i = 1; i < enemies.Length; i++)
                {
                    float disCur = Vector3.Distance(closest.gameObject.transform.position, proyectile.transform.position);
                    float disPot = Vector3.Distance(enemies[i].gameObject.transform.position, proyectile.transform.position);

                    if (disPot < disCur)
                    {
                        closest = enemies[i];
                    }
                }
            }

            GameObject _mini = Instantiate(miniProyectile, proyectile.transform.position, Quaternion.identity);

            Vector3 dir = closest.transform.position - _mini.transform.position;

            _mini.transform.up = dir;
        }
    }

    public Effect ComboBaseParasite()
    {
        return new Effect(TypeOfEffect.Mutate, (int)SeedTypes.Base);
    }

    public Effect ComboRootParasite()
    {
        return new Effect(TypeOfEffect.Mutate, (int)SeedTypes.Root);
    }

    public Effect ComboBouncerParasite()
    {
        return new Effect(TypeOfEffect.Mutate, (int)SeedTypes.Bouncer);
    }

    public Effect ComboSeekerParasite()
    {
        return new Effect(TypeOfEffect.Mutate, (int)SeedTypes.Seeker);
    }

    public Effect ComboRootExplosive()
    {
        return new Effect(TypeOfEffect.DamageOverTime, 1f);
    }

    public Effect ComboExplosiveParasite()
    {
        return new Effect(TypeOfEffect.TickBoom, 2f);
    }

    public Effect ComboBouncerSeeker()
    {
        return new Effect(TypeOfEffect.Masochism, 3f);
    }
}
