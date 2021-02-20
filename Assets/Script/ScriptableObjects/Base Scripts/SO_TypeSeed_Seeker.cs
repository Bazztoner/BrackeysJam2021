using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seeker", menuName = "New Seeker Seed")]
public class SO_TypeSeed_Seeker : SO_TypeSeed_Generic
{
    [Range(0f, .9f)]
    public float precisionChange;

    public Vector3 Seek(int amount, Transform tfmProyectil)
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

            float _aimInit = .005f;
            float _aimCorrected = _aimInit;
            float _aim = 0f;

            for (int i = 1; i < amount; i++)
            {
                _aimInit *= precisionChange;
                _aimCorrected += _aimInit;
            }

            _aim += _aimCorrected;

            Vector3 dirInit = dir;
            Vector3 dirEnd = Vector3.Normalize(closestEnemy - tfmProyectil.position);

            dir = Vector3.Lerp(dirInit, dirEnd, _aim);
        }

        return dir;
    }
}
