using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedRooter : BaseTree
{
    [SerializeField]
    protected Effect root = new Effect(TypeOfEffect.Stun, 2f);

    public override void Behave() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _col = collision.gameObject;

        if (_col.GetComponent<EnemyBase>())
        {
            _col.GetComponent<EnemyBase>().RecieveEffect(root);
            Destroy(gameObject);
        }
    }
}
