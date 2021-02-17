using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seeker", menuName = "New Seeker Seed")]
public class SO_TypeSeed_Seeker : SO_TypeSeed_Generic
{
    [Range(0f, .9f)]
    public float initialPrecision;

    [Range(0f, .9f)]
    public float precisionChange;

    public Vector3 Seek()
    {
        Vector3 dir = tfmProyectil.up;

        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

        if (enemies.Length >= 1)
        {
            Vector3 closestEnemy = enemies[0].gameObject.transform.position;

            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 curEnemy = enemies[i].gameObject.transform.position;

                if (Vector3.Distance(curEnemy, tfmProyectil.position) < Vector3.Distance(closestEnemy, tfmProyectil.position))
                {
                    closestEnemy = curEnemy;
                }
            }

            float _realAim = initialPrecision;
            float _precision = precisionChange;

            for (int i = 1; i < amount; i++)
            {
                _precision = (_precision * precisionChange);
                _realAim += _precision;
            }

            Vector3 dirInit = dir;
            Vector3 dirEnd = Vector3.Normalize(closestEnemy - tfmProyectil.position);

            dir = Vector3.Lerp(dirInit, dirEnd, _realAim);
        }

        return dir;
    }
}
