using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedParasite : BaseTree
{
    [SerializeField]
    protected GameObject collectible;

    public int countdown = 3;

    public override void Behave() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _col = collision.gameObject;

        if (_col.GetComponent<BaseProyectile>())
        {
            Destroy(_col);

            countdown--;

            if (countdown <= 0)
            {
                Instantiate(collectible, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
