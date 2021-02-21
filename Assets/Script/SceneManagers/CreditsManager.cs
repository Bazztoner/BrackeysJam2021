using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    protected Image bg;

    [SerializeField]
    protected Transform tfmText, posTextInit, posTextEnd;

    [SerializeField]
    protected TextMeshProUGUI txtCredits;

    [SerializeField]
    protected float tickLightUp, tickTextIn, tickTextScroll;

    void Start()
    {
        StartCoroutine(RollCredits());
    }

    IEnumerator RollCredits()
    {
        float t = 0f;

        Color colorInit = Color.black;
        Color colorEnd = new Color(0f, 0f, 0f, .15f);

        while (t < 1f)
        {
            bg.color = Color.Lerp(colorInit, colorEnd, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickLightUp;
        }

        t = 0f;

        colorInit = new Color(1f, 1f, 1f, 0f);
        colorEnd = Color.white;

        while (t < 1f)
        {
            txtCredits.color = Color.Lerp(colorInit, colorEnd, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickTextIn;
        }

        t = 0f;

        while (t < 1f)
        {
            tfmText.position = Vector3.Lerp(posTextInit.position, posTextEnd.position, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickTextScroll;
        }

        t = 0f;

        colorInit = new Color(0f, 0f, 0f, .15f); 
        colorEnd = Color.black;

        while (t < 1f)
        {
            bg.color = Color.Lerp(colorInit, colorEnd, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickLightUp;
        }

        t = 0f;

        colorInit = Color.white; 
        colorEnd = new Color(1f, 1f, 1f, 0f);

        while (t < 1f)
        {
            txtCredits.color = Color.Lerp(colorInit, colorEnd, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickTextIn;
        }

        BackToMenu();
    }

    public void BackToMenu()
    {
        GameManager.Instance.LoadScene(GameScenes.Menu);
    }
}
