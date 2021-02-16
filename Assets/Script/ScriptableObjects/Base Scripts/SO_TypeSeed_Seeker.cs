using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seeker", menuName = "New Seeker Seed")]
public class SO_TypeSeed_Seeker : SO_TypeSeed_Generic
{
    float perfectAim;

    float precision;

    public override void Define(int amount, Proyectile _proyectile)
    {
        perfectAim = (parameter * 5f);

        base.Define(amount, _proyectile);

        precision = parameter;
    }

    public override void Create()
    {
        throw new System.NotImplementedException();
    }

    public override void Traverse()
    {
        float realAim = (1f / perfectAim) * precision;

        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

        Vector3 closestEnemy = enemies[0].gameObject.transform.position;

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 curEnemy = enemies[i].gameObject.transform.position;

            if (Vector3.Distance(closestEnemy, tfmProyectil.position) < Vector3.Distance(curEnemy, tfmProyectil.position))
            {
                closestEnemy = enemies[i].gameObject.transform.position;
            }
        }

        Vector3 dir = closestEnemy - tfmProyectil.position;

        Quaternion rotEnd = Quaternion.LookRotation(Vector3.zero, dir);

        tfmProyectil.rotation = Quaternion.Lerp(tfmProyectil.rotation, rotEnd, realAim);
    }

    public override void Impact()
    {
        throw new System.NotImplementedException();
    }
}
