using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIAvailableSeed : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI ammo;

    [SerializeField]
    protected SeedTypes type;

    GameObject _lockGraphic;

    void Start()
    {
        if (ammo == null) ammo = GetComponentInChildren<TextMeshProUGUI>();
        _lockGraphic = GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Lock").Select(x =>x.gameObject).FirstOrDefault();
    }

    void Update()
    {
        _lockGraphic.SetActive(GameManager.Instance.GetUnlockedSeeds()[(int)type]);
    }

    public void UpdateAmmo()
    {
        ammo.text = GameManager.Instance.GetRemainingSeeds()[(int)type].ToString();
    }
}
