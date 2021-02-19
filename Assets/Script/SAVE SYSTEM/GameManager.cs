using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [SerializeField]
    GameObject collectableSeeds;

    [SerializeField]
    int[] maxSeedsAmount = new int[] { 100, 50, 25, 50, 30, 15 };

    bool[] unlockedSeeds = new bool[6] { false, false, false, false, false, false };

    bool[] unlockedPlanets = new bool[6] { true, false, false, false, false, false };

    int[] totalSeeds = new int[6] { 100, 0, 0, 0, 0, 0 };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        manager = this;
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
    #endregion
}
