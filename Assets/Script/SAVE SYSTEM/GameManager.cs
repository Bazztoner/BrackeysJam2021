using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [SerializeField]
    int[] maxSeedsAmount = new int[] { 100, 50, 25, 50, 30, 15 };

    bool[] unlockedSeeds = new bool[6] { false, false, false, false, false, false };

    bool[] unlockedPlanets = new bool[6] { true, false, false, false, false, false };

    int[] totalSeeds = new int[6] { 100, 0, 0, 0, 0, 0 };

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        manager = this;
    }
}
