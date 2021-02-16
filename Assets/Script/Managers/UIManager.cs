using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
	static UIManager _instance;
	public static UIManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<UIManager>();
				if(_instance == null)
				{
					_instance = new GameObject("new UIManager Object").AddComponent<UIManager>().GetComponent<UIManager>();
				}
			}
			return _instance;
		}
	}

	public Image hpBar;

	public void UpdateHPBar(int currentHP, int maxHP)
    {
		float fillAmount = (float)currentHP / (float)maxHP;

		hpBar.fillAmount = fillAmount;
    }
}
