using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfTypeSeedRooter : MonoBehaviour
{
    [SerializeField]
    protected Effect root = new Effect(TypeOfEffect.Stun, 2f);

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
