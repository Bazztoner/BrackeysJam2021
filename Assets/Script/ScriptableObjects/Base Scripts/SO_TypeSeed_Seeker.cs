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

            float _perfectAim = 1f * Time.fixedDeltaTime;
            float _realAim = _perfectAim / 5f;
            float _precision = precisionChange;

            for (int i = 0; i < amount; i++)
            {
                _precision = (_realAim * precisionChange);
                _realAim += _precision;
            }

            Debug.Log($"Real aim for {amount} is {_realAim}");

            Vector3 dirInit = dir;
            Vector3 dirEnd = Vector3.Normalize(closestEnemy - tfmProyectil.position);

            dir = Vector3.Lerp(dirInit, dirEnd, _realAim);
        }

        return dir;
    }
}
