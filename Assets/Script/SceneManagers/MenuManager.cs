using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    Image frontground;

    [SerializeField]
    protected float tickLightUp;

    [SerializeField]
    protected List<Option> options;

    protected int iOption;

    Coroutine lightUp, listen;

    MenuControl input;
    
    public enum SelectedOption { Continue, NewGame, Credits, Quit }

    private void Awake()
    {
        input = new MenuControl();
    }

    void Start()
    {
        input.Navigation.NavigateUp.performed += _ => NavigateUp();
        input.Navigation.NavigateDown.performed += _ => NavigateDown();
        input.Navigation.Select.performed += _ => options[iOption].onCall.Invoke();

        lightUp = StartCoroutine(LightUp());
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    IEnumerator LightUp()
    {
        float t = 0f;

        Color end = new Color(0f, 0f, 0f, 0f);

        while (t < 1f)
        {
            frontground.color = Color.Lerp(Color.black, end, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickLightUp;
        }

        EnableMenu();
    }

    void EnableMenu()
    {
        if (GameManager.Instance.GetPlanetUnlocked(0))
        {
            options[0].button.SetActive(true);
            iOption = 0;
        }
        else iOption = 1;

        for (int i = 1; i < options.Count; i++)
        {
            options[i].button.SetActive(true);
        }

        options[iOption].Select();
    }

    void NavigateUp()
    {
        options[iOption].Unselect();

        iOption++;

        if (iOption >= options.Count)
        {
            if (GameManager.Instance.GetPlanetUnlocked(0))
                iOption = 0;
            else iOption = 1;
        }

        options[iOption].Select();
    }

    void NavigateDown()
    {
        options[iOption].Unselect();

        iOption--;

        if ((GameManager.Instance.GetPlanetUnlocked(0) && (iOption < 0)) || (!GameManager.Instance.GetPlanetUnlocked(0) && (iOption == 0)))
            iOption = (options.Count - 1);

        options[iOption].Select();
    }

    public void Continue()
    {
        StartCoroutine(MoveToAnotherScene(1));
    }

    public void NewGame()
    {
        StartCoroutine(MoveToAnotherScene(2));
    }

    public void ShowCredits()
    {
        StartCoroutine(MoveToAnotherScene(3));
    }

    public void LeaveGame()
    {
        StartCoroutine(MoveToAnotherScene(4));
    }

    IEnumerator MoveToAnotherScene(int _i)
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].button.SetActive(false);
        }

        float t = 0f;

        Color init = new Color(0f, 0f, 0f, 0f);

        while (t < 1f)
        {
            frontground.color = Color.Lerp(init, Color.black, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * tickLightUp;
        }

        switch (_i)
        {
            case 1:
                GameManager.Instance.LoadScene(GameScenes.HUB);
                break;
            case 2:
                GameManager.Instance.ResetGame();
                GameManager.Instance.LoadScene(GameScenes.HUB);
                break;
            case 3:
                GameManager.Instance.LoadScene(GameScenes.Credits);
                break;
            case 4:
                GameManager.Instance.Quit();
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class Option
{
    [SerializeField]
    public GameObject button;

    [SerializeField]
    protected Image imgOption;

    [SerializeField]
    protected Sprite sprSelected, sprUnselected;

    public UnityEvent onCall;

    bool isEnabled = false;

    public void Enable()
    {
        isEnabled = true;
    }

    public bool GetIsEnable()
    {
        return isEnabled;
    }

    public void Select()
    {
        imgOption.sprite = sprSelected;
    }

    public void Unselect()
    {
        imgOption.sprite = sprUnselected;
    }
}
