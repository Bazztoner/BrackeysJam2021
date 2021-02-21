using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class MusicManager : MonoBehaviour
{
    static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("new MusicManager Object").AddComponent<MusicManager>().GetComponent<MusicManager>();
                    _instance.FillFields();
                }
            }
            return _instance;
        }
    }

    AudioSource _src;
    public AudioClip menu, level1, level2, finalStart, finalBoss, win;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        _src = GetComponent<AudioSource>();
    }

    void FillFields()
    {
        _instance.gameObject.AddComponent<AudioSource>();
        var clips = Resources.LoadAll<AudioClip>("Sound/Music/");
        menu = clips.First(x => x.name == "TREEraforming - Menu");
        level1 = clips.First(x => x.name == "TREEraforming - Level 1");
        level2 = clips.First(x => x.name == "TREEraforming - Level 2");
        finalStart = clips.First(x => x.name == "TREEraforming - End level start");
        finalBoss = clips.First(x => x.name == "TREEraforming - Final Boss");
        win = clips.First(x => x.name == "TREEraforming - Win");
    }

    public void OnStartLoadingScene(GameScenes scene)
    {
        if (GameManager.Instance.CurrentScene != GameScenes.Menu &&
            GameManager.Instance.CurrentScene != GameScenes.Splash &&
            GameManager.Instance.CurrentScene != GameScenes.Credits)
        {
            StartCoroutine(ProgressiveMuting());
        }
    }

    IEnumerator ProgressiveMuting()
    {
        var currentTime = 0f;

        while (currentTime <= 1)
        {
            _src.volume = Mathf.Lerp(1, 0, currentTime);
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
        }
    }

    public void OnChangeScene(GameScenes scene)
    {
        StopAllCoroutines();
        _src.volume = 1;

        if (GameManager.Instance.CurrentScene != GameScenes.Menu &&
            GameManager.Instance.CurrentScene != GameScenes.Splash &&
            GameManager.Instance.CurrentScene != GameScenes.Credits)
        {
            switch (scene)
            {
                case GameScenes.P1L1:
                    _src.clip = level1;
                    break;
                case GameScenes.P1L2:
                    _src.clip = level2;
                    break;
                case GameScenes.P2L1:
                    _src.clip = level1;
                    break;
                case GameScenes.P2L2:
                    _src.clip = level2;
                    break;
                case GameScenes.P3L1:
                    _src.clip = level1;
                    break;
                case GameScenes.P3L2:
                    _src.clip = level2;
                    break;
                case GameScenes.P4L1:
                    _src.clip = level1;
                    break;
                case GameScenes.P4L2:
                    _src.clip = level2;
                    break;
                case GameScenes.P5L1:
                    _src.clip = level1;
                    break;
                case GameScenes.P5L2:
                    _src.clip = level2;
                    break;
                case GameScenes.FinalBoss:
                    _src.clip = null;
                    break;
                case GameScenes.WinScreen:
                    _src.clip = win;
                    break;
                default:
                    _src.clip = null;
                    break;
            }
        }
        else if(scene == GameScenes.Splash)
        {
            _src.clip = menu;
        }
    }
}
