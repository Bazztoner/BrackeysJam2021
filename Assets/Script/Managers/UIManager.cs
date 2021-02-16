﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("new UIManager Object").AddComponent<UIManager>().GetComponent<UIManager>();
                }
            }
            return _instance;
        }
    }

    public Image hpBar;

    public Image[] comboSlots;
    Image[] _comboImages;

    [Header("In order like the SeedTypes enum")]
    public Sprite[] seedImages;

    void Awake()
    {
        _comboImages = new Image[comboSlots.Length];
        for (int i = 0; i < comboSlots.Length; i++)
        {
            _comboImages[i] = comboSlots[i].GetComponentsInChildren<Image>(true).Where(x => x != comboSlots[i]).First();
            _comboImages[i].enabled = false;
        }
    }

    public void UpdateHPBar(int currentHP, int maxHP)
    {
        float fillAmount = (float)currentHP / (float)maxHP;

        hpBar.fillAmount = fillAmount;
    }

    public void AddSeedToCombo(int i, SeedTypes seed)
    {
        _comboImages[i].enabled = true;
        _comboImages[i].sprite = seedImages[(int)seed];
    }

    public void ClearCombo()
    {
        foreach (var item in _comboImages)
        {
            item.sprite = null;
            item.enabled = false;
        }
    }
}
