using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forestation : MonoBehaviour
{
    [SerializeField]
    protected List<Transform> sprouts = new List<Transform>();

    [SerializeField]
    protected List<GameObject> trees = new List<GameObject>();

    public void Sprout(List<SeedTypes> seeds)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            BaseTree _tree = Instantiate(trees[(int)seeds[i]], sprouts[i].position, Quaternion.identity).GetComponent<BaseTree>();

            _tree.Sprout();

            //Agregar control gráfico que muestre el Srpite correspondiente.
        }
    }
}
