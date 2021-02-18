using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedExplosive : BaseTree
{
    public Effect dot = new Effect(TypeOfEffect.DamageOverTime, 3f, 1f);

    public TreeOfTypeSeedExplosive(Effect dot)
    {
        this.dot = dot;
    }

    public override void Behave() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _col = collision.gameObject;

        if (_col.GetComponent<EnemyBase>())
        {
            _col.GetComponent<EnemyBase>().RecieveEffect(dot);
        }
    }
}
