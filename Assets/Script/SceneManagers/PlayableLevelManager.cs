using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayableLevelManager : MonoBehaviour
{
    [SerializeField]
    protected PlayerController player;

    [SerializeField]
    protected Image foreground;

    [SerializeField]
    protected Image txtDefeat;

    [SerializeField]
    protected Transform pointVictory;

    [SerializeField]
    protected SeedPickUp pickUp;

    public GameScenes nextScene;

    public UnityEvent onWin;

    void Start()
    {
        player.onDeath.AddListener(delegate { EndLevel(false); });

        if (pickUp != null) pickUp.onPickUp.AddListener(delegate { UnlockSeed(pickUp.GetTypeSeed()); });

        StartCoroutine(LightUp());
    }

    public void EndLevel(bool victory)
    {
        StartCoroutine(LightDown(victory));
    }

    IEnumerator LightUp()
    {
        Color end = new Color(0f, 0f, 0f, 0f);

        float t = 0f;

        while (t < 1f)
        {
            foreground.color = Color.Lerp(Color.black, end, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;

        }
    }

    IEnumerator LightDown(bool victory)
    {
        if (victory)
        {
            Vector3 initPos = player.transform.position;
            Vector3 endPos = pointVictory.position;

            Color init = new Color(0f, 0f, 0f, 0f);

            float t = 0f;
            float _t = 0f;

            while (_t < 1f)
            {
                player.transform.position = Vector3.Lerp(initPos, pointVictory.position, t);

                yield return new WaitForEndOfFrame();

                if (t < 1f) t += Time.deltaTime;

                if (t > .5f)
                {
                    foreground.color = Color.Lerp(init, Color.black, _t);

                    yield return new WaitForEndOfFrame();

                    _t += Time.deltaTime;
                }
            }

            yield return new WaitForSeconds(.25f);

            onWin.Invoke();

            GameManager.Instance.LoadScene(nextScene);
        }
        else
        {
            Color init = new Color(0f, 0f, 0f, 0f);

            float t = 0f;
            float _t = 0f;

            while (_t < 1f)
            {
                foreground.color = Color.Lerp(init, Color.black, _t);

                yield return new WaitForEndOfFrame();

                if (t < 1f) t += Time.deltaTime;

                if (t > .5f)
                {
                    txtDefeat.color = Color.Lerp(init, Color.white, _t);

                    yield return new WaitForEndOfFrame();

                    _t += Time.deltaTime;
                }
            }

            yield return new WaitForSeconds(2.5f);

            GameManager.Instance.LoadScene(GameScenes.Menu);
        }
    }

    public void UnlockPlanet(int _planet)
    {
        GameManager.Instance.UnlockPlanet(_planet);
    }

    public void UnlockSeed(int _seed)
    {
        GameManager.Instance.UnlockSeed(_seed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
            EndLevel(true);
    }
}