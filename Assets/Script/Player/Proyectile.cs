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

    List<Effect> effects;

    [SerializeField]
    LayerMask enemyLayer, scenaryLayer;

    event Action onCreate, onImpact, onTraverse;

    public void DefineCombo(SeedTypes type, int amount)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            if (i == (int) type)
            {
                seeds[i].Define(amount, this);
            }
        }

        switch (type)
        {
            case SeedTypes.Base:
                effects.Add(seeds[0].specialEffect);
                break;
            case SeedTypes.Root:
                effects.Add(seeds[1].specialEffect);
                break;
            case SeedTypes.Explosive:
                effects.Add(seeds[2].specialEffect);
                break;
            case SeedTypes.Bouncer:
                onImpact += seeds[3].Impact;
                break;
            case SeedTypes.Seeker:
                onTraverse += seeds[4].Traverse;
                break;
            case SeedTypes.Parasite:
                effects.Add(seeds[5].specialEffect);
                break;
            default:
                break;
        }
    }

    public void DefineCombination(SeedTypes _i, SeedTypes _j)
    {
        switch (_i)
        {
            case SeedTypes.Base:
                switch (_j)
                {
                    case SeedTypes.Root:
                        combos.ComboBaseRoot(); // Modify SeedBase
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
                switch (_j)
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
                switch (_j)
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
                switch (_j)
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
                switch (_j)
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
            if (i.Equals((int)type))
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

        List<int> repetitions = new List<int>();

        List<SeedTypes> seedDamage = new List<SeedTypes>();

        int repIndex = 0;

        seedDamage.Add(comboSeeds[0]);

        repetitions.Add(1);

        for (int i = 1; i < comboSeeds.Count; i++)
        {
            if (comboSeeds[i].Equals(comboSeeds[i - 1]))
            {
                repetitions[repIndex]++;
            }
            else
            {
                repetitions.Add(1);
                seedDamage.Add(comboSeeds[i]);
                repIndex++;
            }
        }

        for (int i = 0; i < repetitions.Count; i++)
        {
            for (int j = 0; j < repetitions[i]; j++)
            {
                damage += (seeds[(int)seedDamage[i]].damage - j);
            }
        }
    }

    public void OnImpact()
    {
        if (onImpact != null)
        {
            onImpact();
        }
    }

    public void OnTraverse()
    {
        if (onTraverse != null)
        {
            onTraverse();
        }
    }

    public void OnCreate()
    {
        if (onCreate != null)
        {
            OnCreate();
        }
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.LayerMatchesWith(enemyLayer.value))
        {
            EnemyBase _enemy = collision.collider.gameObject.GetComponent<EnemyBase>();

            _enemy.RecieveEffect(new Effect(TypeOfEffect.Damage, damage));

            for (int i = 0; i < effects.Count; i++)
            {
                _enemy.RecieveEffect(effects[i]);
            }
        }
        
        OnImpact();
    }

    private void Update()
    {
        OnTraverse();
    }
}
