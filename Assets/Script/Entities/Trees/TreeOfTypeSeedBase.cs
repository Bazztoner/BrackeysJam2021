using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedBase : BaseTree
{
    public int life = 3;

    public override void Behave() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _col = collision.gameObject;

        if (_col.GetComponent<BaseProyectile>())
        {
            Destroy(_col);

            life--;

            if (life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
