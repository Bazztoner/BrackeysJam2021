using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUBManager : MonoBehaviour
{
    [SerializeField]
    protected Transform player;

    [SerializeField]
    protected Transform[] pointsStarting = new Transform[6];

    [SerializeField]
    protected PlanetOrbit[] planets;

    [SerializeField]
    protected Image rend;

    [SerializeField]
    protected GameObject hud;

    private void Start()
    {
        for (int i = 0; i < pointsStarting.Length; i++)
        {
            if (GameManager.Instance.GetPlanetUnlocked(i))
            {
                planets[i].gameObject.SetActive(false);
                player.position = pointsStarting[i].position;
            }
            else break;
        }

        StartCoroutine(LightUp());
    }

    IEnumerator LightUp()
    {
        Color end = new Color(0f, 0f, 0f, 0f);

        float t = 0f;

        while (t < 1f)
        {
            rend.color = Color.Lerp(Color.black, end, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;
        }

        hud.SetActive(true);
    }

    public void Transitioin(int i)
    {
        StartCoroutine(LightDown(i));
    }

    IEnumerator LightDown(int i)
    {
        Color init = new Color(0f, 0f, 0f, 0f);

        float t = 0f;

        while (t < 1f)
        {
            rend.color = Color.Lerp(init, Color.black, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;
        }

        EnterPlanet(i);
    }

    public void EnterPlanet(int i)
    {
        switch (i)
        {
            case 0:
                GameManager.Instance.LoadScene(GameScenes.P1L1);
                break;
            case 1:
                GameManager.Instance.LoadScene(GameScenes.P2L1);
                break;
            case 2:
                GameManager.Instance.LoadScene(GameScenes.P3L1);
                break;
            case 3:
                GameManager.Instance.LoadScene(GameScenes.P4L1);
                break;
            case 4:
                GameManager.Instance.LoadScene(GameScenes.P5L1);
                break;
            case 5:
                GameManager.Instance.LoadScene(GameScenes.FinalBoss);
                break;
            default:
                break;
        }
    }
}
