using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : BaseProjectile
{
    [SerializeField]
    List<SO_TypeSeed_Generic> seeds;

    [SerializeField]
    SO_SeedCombos combos;

    public static event Action onCreate, onTraverse, onImpact;

    public void DefineCombo(SeedTypes type, int amount)
    {
        switch (type)
        {
            // Revisar los comportamientos específicos
            // y asignarlos como EventListeners cuando corresponda.

            case SeedTypes.Base:
                onCreate += seeds[0].Define;
                break;
            case SeedTypes.Root:
                onCreate += seeds[0].Define;
                break;
            case SeedTypes.Explosive:
                onCreate += seeds[0].Define;
                break;
            case SeedTypes.Bouncer:
                onCreate += seeds[0].Define;
                break;
            case SeedTypes.Seeker:
                onCreate += seeds[0].Define;
                break;
            case SeedTypes.Parasite:
                onCreate += seeds[0].Define;
                break;
        }
    }

    public void DefineCombination(SeedTypes i, SeedTypes j)
    {
        switch (i)
        {
            case SeedTypes.Base:
                switch (j)
                {
                    case SeedTypes.Root:
                        combos.ComboBaseRoot();
                        break;
                    case SeedTypes.Explosive:
                        combos.ComboBaseExplosive();
                        break;
                    case SeedTypes.Bouncer:
                        combos.ComboBaseBouncer();
                        break;
                    case SeedTypes.Seeker:
                        combos.ComboBaseSeeker();
                        break;
                    case SeedTypes.Parasite:
                        combos.ComboBaseParasite();
                        break;
                }
                break;
            case SeedTypes.Root:
                switch (j)
                {
                    case SeedTypes.Explosive:
                        combos.ComboRootExplosive();
                        break;
                    case SeedTypes.Bouncer:
                        combos.ComboRootBouncer();
                        break;
                    case SeedTypes.Seeker:
                        combos.ComboRootSeeker();
                        break;
                    case SeedTypes.Parasite:
                        combos.ComboRootParasite();
                        break;
                }
                break;
            case SeedTypes.Explosive:
                switch (j)
                {
                    case SeedTypes.Bouncer:
                        combos.ComboExplosiveBouncer();
                        break;
                    case SeedTypes.Seeker:
                        combos.ComboExplosiveSeeker();
                        break;
                    case SeedTypes.Parasite:
                        combos.ComboExplosiveParasite();
                        break;
                }
                break;
            case SeedTypes.Bouncer:
                switch (j)
                {
                    case SeedTypes.Seeker:
                        combos.ComboBouncerSeeker();
                        break;
                    case SeedTypes.Parasite:
                        combos.ComboBouncerParasite();
                        break;
                }
                break;
            case SeedTypes.Seeker:
                switch (j)
                {
                    case SeedTypes.Parasite:
                        combos.ComboSeekerParasite();
                        break;
                }
                break;
        }
    }

    public void SetSpeed(SeedTypes type)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            if (seeds[i].Equals(type))
            {
                if (seeds[i].speed > speed) speed = seeds[i].speed;
            }
        }
    }

    public void SetDamage(SeedTypes[] _seeds)
    {
        int[] amounts = new int[6] { 0, 0, 0, 0, 0, 0 };

        for (int i = 0; i < _seeds.Length; i++)
        {
            for (int j = 0; j < seeds.Count; j++)
            {
                if (_seeds[i].Equals((SeedTypes) j))
                {
                    amounts[j]++;
                }
            }
        }

        List<float> damages = new List<float>();

        List<SeedTypes> comboSeeds = new List<SeedTypes>();

        for (int i = 0; i < amounts.Length; i++)
        {
            if (amounts[i] > 0)
            {
                for (int j = 0; j < amounts[i]; j++)
                {
                    damages.Add(seeds[i].damage);
                    comboSeeds.Add((SeedTypes)i);
                }
            }
        }

        for (int i = 1; i < damages.Count; i++)
        {
            for (int j = 0; j < damages.Count - 1; j++)
            {
                float _dmg;
                SeedTypes _st;

                if (damages[i] > damages[j])
                {
                    _dmg = damages[j];
                    _st = comboSeeds[j];

                    damages[j] = damages[i];
                    comboSeeds[j] = comboSeeds[i];

                    damages[i] = _dmg;
                    comboSeeds[i] = _st;
                }
            }
        }

        int repetition;

        for (int i = 0; i < comboSeeds.Count; i++)
        {

        }
    }

    void OnCreate()
    {
        if (onCreate != null) onCreate();
    }

    void OnTraverse()
    {
        if (onTraverse != null) onTraverse();
    }

    void OnImpact()
    {
        if (onImpact != null) onImpact();
    }
}
