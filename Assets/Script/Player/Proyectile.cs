using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Proyectile : BaseProyectile
{
    public SO_TypeSeed_Base seedBase;


    public SO_TypeSeed_Rooter seedRooter;


    public SO_TypeSeed_Explosive seedExplosive;


    public SO_TypeSeed_Bouncer seedBouncer;


    public SO_TypeSeed_Seeker seedSeeker;


    public SO_TypeSeed_Parasite seedParasite;

    public SO_SeedCombos combos;

    public GameObject foresation;

    List<Effect> effects = new List<Effect>();

    List<SeedTypes> plants = new List<SeedTypes>();

    int[] seedAmounts = new int[6];

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
        seedAmounts[(int)type] += amount;

        Debug.Log($"Total amount of type-seed {type} is {seedAmounts[(int)type]}");

        switch (type)
        {
            case SeedTypes.Base:
                effects.Add(seedBase.GetKnockBack(seedAmounts[0], gameObject.transform));
                plants.Add(SeedTypes.Base);
                break;
            case SeedTypes.Root:
                effects.Add(seedRooter.GetStun(seedAmounts[(int)SeedTypes.Root]));
                plants.Add(SeedTypes.Root);
                break;
            case SeedTypes.Explosive:
                onImpactEnemy.AddListener(delegate { seedExplosive.AOE(seedAmounts[(int)SeedTypes.Explosive], gameObject.transform); });
                plants.Add(SeedTypes.Explosive);
                break;
            case SeedTypes.Bouncer:
                life = seedBouncer.GetBounces(seedAmounts[(int)SeedTypes.Bouncer]);
                onImpactEnvironment.AddListener(Bounce);
                plants.Add(SeedTypes.Bouncer);
                break;
            case SeedTypes.Seeker:
                onTraverse.AddListener(ChangeDirection);
                plants.Add(SeedTypes.Seeker);
                break;
            case SeedTypes.Parasite:
                effects.Add(seedParasite.GetMindControl(seedAmounts[(int)SeedTypes.Parasite]));
                plants.Add(SeedTypes.Parasite);
                break;
            default:
                break;
        }

        Initialize();
    }

    public void DefineCombination(SeedTypes _i, SeedTypes _j)
    {
        switch (_i)
        {
            case SeedTypes.Base:
                switch (_j)
                {
                    case SeedTypes.Explosive:
                        onCreate.AddListener(delegate { combos.ComboBaseExplosive(gameObject); });
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
                        onImpactEnemy.AddListener(delegate { combos.ComboRootBouncer(gameObject); });
                        break;
                    case SeedTypes.Seeker:
                        onImpactEnemy.AddListener(delegate { combos.ComboRootSeeker(gameObject); });
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
                        onImpactEnemy.AddListener(delegate { combos.ComboExplosiveBouncer(gameObject); });
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
                if (_seeds[i].Equals((SeedTypes)j))
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

        Effect makeDamage = new Effect(TypeOfEffect.Damage, damage, transform.position);

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

            if (effects.Count >= 1 && _enemy != null)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    _enemy.RecieveEffect(effects[i]);
                }
            }

            onImpactEnemy.Invoke();
        }

        if (collider.gameObject.LayerMatchesWith("Environment"))
        {
            EnvironmentHit();
        }
    }

    protected override void FixedUpdate()
    {
        onTraverse.Invoke();

        base.FixedUpdate();
    }

    void EnemyHit()
    {
        Destroy(gameObject);
    }

    void EnvironmentHit()
    {
        onImpactEnvironment.Invoke();

        life--;

        if (life <= 0)
        {
            SproutNewPlants();

            Destroy(gameObject);
        }
    }

    void ChangeDirection()
    {
        Vector3 dir = seedSeeker.Seek(seedAmounts[(int)SeedTypes.Seeker], gameObject.transform);

        transform.up = dir;
    }

    void Bounce()
    {
        Vector3 dir = seedBouncer.Bounce(gameObject.transform);

        transform.up = dir;
    }

    void SproutNewPlants()
    {
        Forestation forest = Instantiate(foresation, transform.position, Quaternion.identity).GetComponent<Forestation>();

        forest.Sprout(plants);
    }
}


