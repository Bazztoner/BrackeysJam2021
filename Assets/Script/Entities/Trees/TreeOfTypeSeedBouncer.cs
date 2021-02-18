using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedBouncer : BaseTree
{
    public float lifeSpan = 3f;

    public override void Behave()
    {
        StartCoroutine(Wither());
    }

    IEnumerator Wither()
    {
        yield return new WaitForSeconds(lifeSpan);

        //Wither Animation?

        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _col = collision.gameObject;

        if (_col.GetComponent<BaseProjectile>())
        {
            Vector3 dir = Vector3.Normalize(col.transform.up - transform.position);
            _col.transform.up = dir;
        }
    }
}
