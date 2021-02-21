using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class P2L2Logic : MonoBehaviour
{
    public GameObject arenaTopCollider, arenaBotCollider, endTrigger;
    public HiveBoss boss;
    bool _bossDead;

    void Start()
    {
        if (boss == null) boss = FindObjectOfType<HiveBoss>();
    }

    void Update()
    {
        CheckBossDeath();
    }

    void CheckBossDeath()
    {
        if (boss == null && !_bossDead)
        {
            arenaTopCollider.SetActive(false);
            arenaBotCollider.SetActive(false);
            endTrigger.SetActive(true);
            _bossDead = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.LayerMatchesWith("Player"))
        {
            arenaBotCollider.SetActive(true);
        }
    }
}
