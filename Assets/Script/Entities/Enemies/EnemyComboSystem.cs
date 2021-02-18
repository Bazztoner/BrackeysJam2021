using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComboSystem : MonoBehaviour
{
    [SerializeField]
    Proyectile_NonEnvironmental prefabProyectile;

    Proyectile_NonEnvironmental _proyectile;

    int[] amount = new int[6];

    public Proyectile_NonEnvironmental DefineCombo(List<SeedTypes> _combo)
    {
        amount = new int[6];

        _proyectile = Instantiate(prefabProyectile);

        SeedTypes[] seeds = _combo.ToArray();

        for (int i = 0; i < seeds.Length; i++)
        {
            amount[(int)seeds[i]]++;
        }

        for (int i = 0; i < amount.Length; i++)
        {
            if (amount[i] >= 1)
            {
                _proyectile.DefineCombo((SeedTypes)i, amount[i]);
                _proyectile.SetSpeed((SeedTypes)i);
            }
        }

        if (seeds.Length > 1)
        {
            for (int i = 0; i < seeds.Length; i++)
            {
                int value = 0;

                for (int j = i + 1; j < seeds.Length; j++)
                {
                    if ((int)seeds[i] > (int)seeds[j])
                    {
                        value = (int)seeds[i];
                        seeds[i] = seeds[j];
                        seeds[j] = (SeedTypes)value;
                    }
                }
            }

            List<SeedTypes> _seeds = new List<SeedTypes>();

            int index = 0;
            _seeds.Add(seeds[0]);

            for (int i = 0; i < seeds.Length - 1; i++)
            {
                if (seeds.Length >= (i + 1))
                {
                    for (int j = i + 1; j < seeds.Length; j++)
                    {
                        if (!seeds[j].Equals(seeds[i]))
                        {
                            _seeds.Add(seeds[j]);
                            index++;
                        }
                    }
                }
            }

            if (_seeds.Count > 1)
            {
                for (int i = 0; i < _seeds.Count; i++)
                {
                    for (int j = i + 1; j < seeds.Length; j++)
                    {
                        _proyectile.DefineCombination(_seeds[i], _seeds[j]);
                    }
                }
            }
        }

        _proyectile.SetDamage(seeds);

        return _proyectile;
    }
}
