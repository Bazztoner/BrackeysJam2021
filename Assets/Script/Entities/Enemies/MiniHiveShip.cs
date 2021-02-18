using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MiniHiveShip : MonoBehaviour
{
	public Transform muzzle;

	int _hp;
	void Start ()
	{
		muzzle = GetComponentsInChildren<Transform>().First(x => x.name == "Muzzle");
	}

	public void InitializeShip(float hp)
    {
		_hp = Mathf.RoundToInt(hp);
    }

	public void TakeDamage(float dmg)
    {
		_hp -= Mathf.RoundToInt(dmg);

        if (_hp <= 0)
        {
			GetComponentInParent<HiveEnemy>().ClearDestroyedShips(this);
			Destroy(gameObject);
        }
    }
}
