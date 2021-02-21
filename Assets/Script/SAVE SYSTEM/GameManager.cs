using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("GameManager Instance").AddComponent<GameManager>().GetComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public GameScenes CurrentScene { get => curScene;}

    [SerializeField]
    GameObject collectableSeeds;

    GameScenes curScene = GameScenes.Splash;

    Coroutine load;

    [SerializeField] int[] maxSeedsAmount = new int[] { 100, 50, 25, 50, 30, 15 };

    bool[] initialUnlockedSeeds = new bool[6] { true, false, false, false, false, false };

    bool[] initialUnlockedPlanets = new bool[6] { false, false, false, false, false, false };

    int[] initialTotalSeeds = new int[6] { 100, 0, 0, 0, 0, 0 };

    [SerializeField] bool[] unlockedSeeds = new bool[6] { true, false, false, false, false, false };

    [SerializeField] bool[] unlockedPlanets = new bool[6] { false, false, false, false, false, false };

    [SerializeField] int[] totalSeeds = new int[6] { 100, 0, 0, 0, 0, 0 };

    int[] initCosts = new int[6] { -1, -1, -1, -1, -1, -1 };

    [SerializeField] int[] costs = new int[6] { -1, -1, -1, -1, -1, -1 };

    public UnityEvent onShoot;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _instance = this;

        unlockedSeeds = initialUnlockedSeeds;
        totalSeeds = initialTotalSeeds;
    }

    #region Set
    public void UnlockPlanet(int i)
    {
        unlockedPlanets[i] = true;
    }

    public void UnlockSeed(int i)
    {
        unlockedSeeds[i] = true;
        totalSeeds[i] = maxSeedsAmount[i];
    }

    public void RemainingSeeds(int[] _seeds)
    {
        totalSeeds = _seeds;
    }

    public void GainSeeds(int i)
    {
        totalSeeds[i]++;
    }

    public void ResetGame()
    {
        unlockedSeeds = initialUnlockedSeeds;
        unlockedPlanets = initialUnlockedPlanets;
        totalSeeds = initialTotalSeeds;
    }
    #endregion

    #region Get
    public bool GetPlanetUnlocked(int i)
    {
        return unlockedPlanets[i];
    }

    public bool[] GetUnlockedSeeds()
    {
        return unlockedSeeds;
    }

    public int[] GetRemainingSeeds()
    {
        return totalSeeds;
    }
    #endregion

    #region GamePlay
    public bool AddToCost(SeedTypes _seed)
    {
        for (int i = 0; i < totalSeeds.Length; i++)
        {
            if ((int)_seed == i && (costs[i] + 2) <= totalSeeds[i])
            {
                costs[i] += 2;

                return true;
            }
        }

        return false;
    }

    public void Consume()
    {
        for (int i = 0; i < totalSeeds.Length; i++)
        {
            if (costs[i] > 0)
            {
                totalSeeds[i] -= costs[i];
            }

            costs[i] = initCosts[i];
        }

        onShoot.Invoke();
    }

    public void SpawnSeeds(Vector3 _pos)
    {
        for (int i = 0; i < unlockedSeeds.Length; i++)
        {
            int maxSeedSpawn = Mathf.RoundToInt(maxSeedsAmount[i] * .25f);

            if (unlockedSeeds[i] && (totalSeeds[i] < ((float)maxSeedsAmount[i] * .75f)))
            {
                int iterations = Mathf.RoundToInt(maxSeedSpawn * (totalSeeds[i] / maxSeedsAmount[i]));

                for (int j = 0; j < iterations; j++)
                {
                    CollectibleSeed _seed = Instantiate(collectableSeeds, _pos, Quaternion.identity).GetComponent<CollectibleSeed>();
                    _seed.SetSeed(i);
                }
            }
        }
    }

    public void LoadScene(GameScenes scene)
    {
        load = StartCoroutine(Load(scene));
        //MusicManager.Instance.OnStartLoadingScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Load(GameScenes scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)scene);

        ao.allowSceneActivation = false;

        while (ao.progress < .9f)
        {
            yield return new WaitForEndOfFrame();
        }
        //MusicManager.Instance.OnChangeScene(scene);
        ao.allowSceneActivation = true;
    }
    #endregion
}

public enum GameScenes
{
    Splash,
    Menu,
    Credits,
    HUB,
    P1L1,
    P1L2,
    P2L1,
    P2L2,
    P3L1,
    P3L2,
    P4L1,
    P4L2,
    P5L1,
    P5L2,
    FinalBoss,
    Victory
}
