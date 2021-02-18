using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedSeeker : BaseTree
{
    [SerializeField]
    protected GameObject miniProjectile;

    public override void Behave()
    {
        StartCoroutine(Bloom());
    }

    IEnumerator Bloom()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

        if (enemies.Length >= 1)
        {
            EnemyBase closestEnemy = enemies[0];

            if (enemies.Length > 1)
            {
                for (int i = 1; i < enemies.Length; i++)
                {
                    float disInit = Vector3.Distance(transform.position, closestEnemy.transform.position);
                    float disCompare = Vector3.Distance(transform.position, enemies[i].transform.position);

                    if (disCompare < disInit)
                    {
                        closestEnemy = enemies[i];
                    }
                }
            }

            MiniProyectile mini = Instantiate(miniProjectile, transform.position, Quaternion.identity).GetComponent<MiniProyectile>();

            mini.SetDestination(closestEnemy.transform);
        }

        yield return new WaitForEndOfFrame();

        //whiter Aniamtion?

        Destroy(gameObject);
    }
}
