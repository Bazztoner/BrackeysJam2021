using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forestation : MonoBehaviour
{
    [SerializeField]
    List<GameObject> sprouts = new List<GameObject>();

    public void Sprout(List<SeedTypes> seeds)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            sprouts[i].SetActive(true);

            //Agregar control gráfico que muestre el Srpite correspondiente.
        }
    }
}
