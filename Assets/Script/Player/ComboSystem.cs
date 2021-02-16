using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [SerializeField]
    GameObject prefabProyectile;

    Proyectile proyectile;

    int[] amount = new int[6];

    public void DefineCombo(Queue<SeedTypes> _combo)
    {
        proyectile = Instantiate(prefabProyectile).GetComponent<Proyectile>();

        SeedTypes[] seeds = _combo.ToArray();

        for (int i = 0; i < seeds.Length; i++)
        {
            switch (seeds[i])
            {
                case SeedTypes.Base:
                    amount[0]++;
                    break;
                case SeedTypes.Root:
                    amount[1]++;
                    break;
                case SeedTypes.Explosive:
                    amount[2]++;
                    break;
                case SeedTypes.Bouncer:
                    amount[3]++;
                    break;
                case SeedTypes.Seeker:
                    amount[4]++;
                    break;
                case SeedTypes.Parasite:
                    amount[5]++;
                    break;
                default:
                    break;
            }

            proyectile.DefineCombo((SeedTypes)i, amount[i]);
            proyectile.SetSpeed((SeedTypes)i);

            if (amount[i] > 0)
            {
                proyectile.SetSpeed((SeedTypes) i);

                for (int j = i + 1; j < seeds.Length; j++)
                {
                    if (amount[j] > 0)
                    {
                        proyectile.DefineCombination((SeedTypes) i, (SeedTypes) j);
                    }
                }
            }
        }

        proyectile.SetDamage(seeds);
    }
}
