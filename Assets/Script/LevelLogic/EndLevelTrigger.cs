using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndLevelTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            FindObjectOfType<PlayableLevelManager>().EndLevel(true);
        }
    }
}
