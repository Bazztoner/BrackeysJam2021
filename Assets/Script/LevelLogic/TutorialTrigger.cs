using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialTrigger : MonoBehaviour
{
	public GameObject[] tutorialPopups;
	public GameObject myTutorial;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            foreach (var item in tutorialPopups)
            {
                item.SetActive(false);
            }
            myTutorial.SetActive(true);
            Destroy(gameObject);
        }
    }
}
