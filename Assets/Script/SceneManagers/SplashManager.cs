using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField]
    protected Image nameAbi, nameIvan, nameLeo, sprpresent, sprTitle;

    Coroutine show;

    private void Start()
    {
        show = StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        float valueInit = 0f;
        float valueEnd = 1f;

        float tAbi = 0f, tIvan = 0f, tLeo = 0f, tpresent = 0f;

        yield return new WaitForSeconds(.75f);

        while (tpresent < 1f)
        {
            nameAbi.color = Color.white * Mathf.Lerp(valueInit, valueEnd, tAbi);

            yield return new WaitForEndOfFrame();

            tAbi += Time.deltaTime;

            if (tAbi > .25f)
            {
                nameIvan.color = Color.white * Mathf.Lerp(valueInit, valueEnd, tIvan);

                yield return new WaitForEndOfFrame();

                tIvan += Time.deltaTime;

                if (tIvan > .25f)
                {
                    nameLeo.color = Color.white * Mathf.Lerp(valueInit, valueEnd, tLeo);

                    yield return new WaitForEndOfFrame();

                    tLeo += Time.deltaTime;

                    if (tLeo >= 1f)
                    {
                        sprpresent.color = Color.white * Mathf.Lerp(valueInit, valueEnd, tpresent);

                        yield return new WaitForEndOfFrame();

                        tpresent += Time.deltaTime;
                    }
                }
            }
        }

        float t = 0f;

        while (t < 1f)
        {
            nameAbi.color = Color.white * Mathf.Lerp(valueEnd, valueInit, t);
            nameIvan.color = Color.white * Mathf.Lerp(valueEnd, valueInit, t);
            nameLeo.color = Color.white * Mathf.Lerp(valueEnd, valueInit, t);
            sprpresent.color = Color.white * Mathf.Lerp(valueEnd, valueInit, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;
        }

        yield return new WaitForSeconds(.25f);

        t = 0f;

        while (t < 1f)
        {
            sprTitle.color = Color.white * Mathf.Lerp(valueInit, valueEnd, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * 2.5f;
        }

        yield return new WaitForSeconds(2.5f);

        t = 0f;

        while (t < 1f)
        {
            sprTitle.color = Color.white * Mathf.Lerp(valueEnd, valueInit, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * 2.5f;
        }

        GameManager.manager.LoadScene(GameScenes.Menu);
    }
}
