using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FinalLevelLogic : MonoBehaviour
{
    public GameObject arenaTopCollider, arenaBotCollider, endTrigger;
    public BossUndestructible boss;
    bool _bossDead;

    void Start()
    {
        if (boss == null) boss = FindObjectOfType<BossUndestructible>();
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
            arenaTopCollider.SetActive(true);
            arenaBotCollider.SetActive(true);
        }
    }
}
