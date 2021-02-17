using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Proyectile : BaseProjectile
{
    public SO_TypeSeed_Base seedBase;


    public SO_TypeSeed_Rooter seedRooter;


    public SO_TypeSeed_Explosive seedExplosive;


    public SO_TypeSeed_Bouncer seedBouncer;


    public SO_TypeSeed_Seeker seedSeeker;


    public SO_TypeSeed_Parasite seedParasite;


    public SO_SeedCombos combos;

    List<Effect> effects = new List<Effect>();

    List<GameObject> plants;

    [SerializeField]
    LayerMask enemyLayer, scenaryLayer;

    int life = 1;

    public UnityEvent onImpactEnemy, onCreate, onTraverse, onImpactEnvironment;

    SO_TypeSeed_Generic Seed(int i)
    {
        switch ((SeedTypes)i)
        {
            case SeedTypes.Base:
                return seedBase;
            case SeedTypes.Root:
                return seedRooter;
            case SeedTypes.Explosive:
                return seedExplosive;
            case SeedTypes.Bouncer:
                return seedBouncer;
            case SeedTypes.Seeker:
                return seedSeeker;
            case SeedTypes.Parasite:
                return seedParasite;
            default:
                return null;
        }
    }

    public void Initialize()
    {
        onImpactEnemy.AddListener(EnemyHit);
    }

    public override void SpawnProjectile(Vector3 position, Vector3 direction, Entity owner)
    {
        base.SpawnProjectile(position, direction, owner);

        onCreate.Invoke();
    }

    public void DefineCombo(SeedTypes type, int amount)
    {
        switch (type)
        {
            case SeedTypes.Base:
                seedBase.Define(amount, this);
                effects.Add(seedBase.GetKnockBack());
                break;
            case SeedTypes.Root:
                seedRooter.Define(amount, this);
                effects.Add(seedRooter.GetStun());
                break;
            case SeedTypes.Explosive:
                seedExplosive.Define(amount, this);
                onImpactEnemy.AddListener(seedExplosive.AOE);
                break;
            case SeedTypes.Bouncer:
                seedBouncer.Define(amount, this);
                life = seedBouncer.GetBounces();
                onImpactEnvironment.AddListener(Bounce);
                break;
            case SeedTypes.Seeker:
                seedSeeker.Define(amount, this);
                onTraverse.AddListener(ChangeDirection);
                break;
            case SeedTypes.Parasite:
                seedParasite.Define(amount, this);
                effects.Add(seedParasite.GetMindControl());
                break;
            default:
                break;
        }

        Initialize();
    } 

    public void DefineCombination(SeedTypes _i, SeedTypes _j)
    {
        combos.Initialize(this);

        switch (_i)
        {
            case SeedTypes.Base:
                switch (_j)
                {
                    case SeedTypes.Explosive:
                        onCreate.AddListener(combos.ComboBaseExplosive);
                        break;
                    case SeedTypes.Parasite:
                        effects.Add(combos.ComboBaseParasite());
                        break;
                }
                break;
            case SeedTypes.Root:
                switch (_j)
                {
                    case SeedTypes.Explosive:
                        effects.Add(combos.ComboRootExplosive());
                        break;
                    case SeedTypes.Bouncer:
                        onImpactEnemy.AddListener(combos.ComboRootBouncer);
                        break;
                    case SeedTypes.Seeker:
                        onImpactEnemy.AddListener(combos.ComboRootSeeker);
                        break;
                    case SeedTypes.Parasite:
                        effects.Add(combos.ComboRootParasite());
                        break;
                }
                break;
            case SeedTypes.Explosive:
                switch (_j)
                {
                    case SeedTypes.Bouncer:
                        onImpactEnemy.AddListener(combos.ComboExplosiveBouncer);
                        break;
                    case SeedTypes.Parasite:
                        effects.Add(combos.ComboExplosiveParasite());
                        break;
                }
                break;
            case SeedTypes.Bouncer:
                switch (_j)
                {
                    case SeedTypes.Seeker:
                        effects.Add(combos.ComboBouncerSeeker());
                        break;
                    case SeedTypes.Parasite:
                        effects.Add(combos.ComboBouncerParasite());
                        break;
                }
                break;
            case SeedTypes.Seeker:
                switch (_j)
                {
                    case SeedTypes.Parasite:
                        effects.Add(combos.ComboSeekerParasite());
                        break;
                }
                break;
        }
    }

    public void SetSpeed(SeedTypes type)
    {
        for (int i = 0; i < 6; i++)
        {
            if (i.Equals((int)type))
            {
                if (Seed(i).speed > speed) speed = Seed(i).speed;
            }
        }
    }

    public void SetDamage(SeedTypes[] _seeds)
    {
        int[] amounts = new int[6] { 0, 0, 0, 0, 0, 0 };

        for (int i = 0; i < _seeds.Length; i++)
        {
            for (int j = 0; j < 6; j++)
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
                    damages.Add(Seed(i).damage);
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
                damage += (Seed((int)seedDamage[i]).damage - j);
            }
        }

        Effect makeDamage = new Effect(TypeOfEffect.Damage, damage);

        effects.Add(makeDamage);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} collided on {collision.collider.gameObject.name}");

        if (collision.gameObject.LayerMatchesWith(enemyLayer.value))
        {
            EnemyBase _enemy = collision.collider.gameObject.GetComponent<EnemyBase>();

            _enemy.RecieveEffect(new Effect(TypeOfEffect.Damage, damage));

            for (int i = 0; i < effects.Count; i++)
            {
                _enemy.RecieveEffect(effects[i]);
            }
        }
        
        //onImpact.Invoke();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.LayerMatchesWith("Enemy") && collider.gameObject != Owner.gameObject)
        {
            EnemyBase _enemy = collider.gameObject.GetComponent<EnemyBase>();

            if (effects.Count >= 1)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    _enemy.RecieveEffect(effects[i]);
                }
            }

            onImpactEnemy.Invoke();
        }
    }

    protected override void FixedUpdate()
    {
        onTraverse.Invoke();

        base.FixedUpdate();
    }

    void EnemyHit()
    {
        onImpactEnemy.Invoke();

        Destroy(gameObject);
    }

    void EnvironmentHit()
    {
        onImpactEnvironment.Invoke();

        life--;

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ChangeDirection()
    {
        Vector3 dir = seedSeeker.Seek();

        transform.up = dir;
    }

    void Bounce()
    {
        Vector3 dir = seedBouncer.Bounce();

        transform.up = dir;
    }
}
