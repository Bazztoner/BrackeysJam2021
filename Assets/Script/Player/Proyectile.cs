using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : BaseProjectile
{
    [SerializeField]
    SO_TypeSeed_Base seedBase;

    [SerializeField]
    SO_TypeSeed_Rooter seedRoot;

    [SerializeField]
    SO_TypeSeed_Explosive seedExplosive;

    [SerializeField]
    SO_TypeSeed_Bouncer seedBouncer;

    [SerializeField]
    SO_TypeSeed_Seeker seedSeeker;

    [SerializeField]
    SO_TypeSeed_Parasyte seedParasite;

    [SerializeField]
    SO_SeedCombos combos;

    public static event Action onCreate, onTraverse, onImpact;

    public void DefineCombo(SeedTypes type, int amount)
    {
        switch (type)
        {
            case SeedTypes.Base:
                onCreate += seedBase.Define;
                break;
            case SeedTypes.Root:
                seedRoot.DefineDamage(amount);
                break;
            case SeedTypes.Explosive:
                seedExplosive.DefineDamage(amount);
                break;
            case SeedTypes.Bouncer:
                seedBouncer.DefineDamage(amount);
                break;
            case SeedTypes.Seeker:
                seedSeeker.DefineDamage(amount);
                break;
            case SeedTypes.Parasite:
                seedParasite.DefineDamage(amount);
                break;
        }
    }

    public void SetSpeed(SeedTypes type)
    {
        switch (type)
        {
            case SeedTypes.Base:
                if (seedBase.speed > speed) speed = seedBase.speed;
                break;
            case SeedTypes.Root:
                if (seedRoot.speed > speed) speed = seedRoot.speed;
                break;
            case SeedTypes.Explosive:
                if (seedExplosive.speed > speed) speed = seedExplosive.speed;
                break;
            case SeedTypes.Bouncer:
                if (seedBouncer.speed > speed) speed = seedBouncer.speed;
                break;
            case SeedTypes.Seeker:
                if (seedSeeker.speed > speed) speed = seedSeeker.speed;
                break;
            case SeedTypes.Parasite:
                if (seedParasite.speed > speed) speed = seedParasite.speed;
                break;
        }
    }

    public void SetDamage(int[] amount)
    {
        float maxDamage = 0f;

        for (int i = 0; i < amount.Length; i++)
        {
            switch ((SeedTypes) i)
            {
                case SeedTypes.Base:
                    if (seedBase.damage > maxDamage) maxDamage = seedBase.damage;
                    break;
                case SeedTypes.Root:
                    if (seedRoot.damage > maxDamage) maxDamage = seedRoot.damage;
                    break;
                case SeedTypes.Explosive:
                    if (seedExplosive.damage > maxDamage) maxDamage = seedExplosive.damage;
                    break;
                case SeedTypes.Bouncer:
                    if (seedBouncer.damage > maxDamage) maxDamage = seedBouncer.damage;
                    break;
                case SeedTypes.Seeker:
                    if (seedSeeker.damage > maxDamage) maxDamage = seedSeeker.damage;
                    break;
                case SeedTypes.Parasite:
                    if (seedParasite.damage > maxDamage) maxDamage = seedParasite.damage;
                    break;
            }
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
