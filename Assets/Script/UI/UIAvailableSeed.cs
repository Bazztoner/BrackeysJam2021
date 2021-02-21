using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAvailableSeed : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI ammo;

    [SerializeField]
    protected SeedTypes type;

    void Start()
    {
        if (ammo == null) ammo = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateAmmo()
    {
        ammo.text = GameManager.Instance.GetRemainingSeeds()[(int)type].ToString();
    }
}
