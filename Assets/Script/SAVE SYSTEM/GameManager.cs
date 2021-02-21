using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    GameObject collectableSeeds;

    GameScenes curScene = GameScenes.Splash;

    Coroutine load;

    [SerializeField]
    int[] maxSeedsAmount = new int[] { 100, 50, 25, 50, 30, 15 };

    [SerializeField] bool[] initialUnlockedSeeds = new bool[6] { true, false, false, false, false, false };

    bool[] initialUnlockedPlanets = new bool[6] { false, false, false, false, false, false };

    [SerializeField] int[] initialTotalSeeds = new int[6] { 100, 0, 0, 0, 0, 0 };

    bool[] unlockedSeeds = new bool[6] { true, false, false, false, false, false };

    bool[] unlockedPlanets = new bool[6] { false, false, false, false, false, false };

    int[] totalSeeds = new int[6] { 100, 0, 0, 0, 0, 0 };

    public bool setDefaultInfo;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        if (setDefaultInfo)
        {
            unlockedSeeds = initialUnlockedSeeds;
            totalSeeds = initialTotalSeeds;
        }
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
    public bool Consume(Queue<SeedTypes> _seeds)
    {
        SeedTypes[] seeds = _seeds.ToArray();

        int[] costs = new int[6];

        for (int i = 0; i < totalSeeds.Length; i++)
        {
            int cost = -1;
            int seed = i;

            for (int j = 0; j < seeds.Length; j++)
            {
                if (j.Equals((SeedTypes)i))
                {
                    cost += 2;
                }

                if (cost > totalSeeds[i]) return false;
                else costs[i] = cost;
            }
        }

        for (int i = 0; i < totalSeeds.Length; i++)
        {
            totalSeeds[i] -= costs[i];
        }

        return true;
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
    FinalBoss
}
