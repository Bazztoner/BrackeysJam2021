using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarager : EnemyBase
{
    [SerializeField]
    GameObject missileBarage;

    [SerializeField]
    Transform[] barageMuzzles = new Transform[8];

    [SerializeField]
    GameObject missileAllAround;

    [SerializeField]
    Transform[] allaroundMuzzles = new Transform[6];

    [SerializeField]
    protected float positionVariation;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float attackeDistance, tickAttack;

    Vector3 left, right;

    Coroutine movement, attack;

    protected override void Start()
    {
        base.Start();

        float minX = transform.position.x - positionVariation;
        float maxX = transform.position.x + positionVariation;

        left = new Vector3(minX, transform.position.y);
        right = new Vector3(maxX, transform.position.y);

        movement = StartCoroutine(Movement());
        attack = StartCoroutine(Attack());
    }

    IEnumerator Movement()
    {
        float t = Vector3.Distance(transform.position, right) / Vector3.Distance(left, right);

        Vector3 curPos = _rb.position;

        bool goingRight = true;

        while (true)
        {
            while (t < 1f)
            {
                if (!_isStunned)
                {
                    _rb.MovePosition(goingRight ? Vector3.Lerp(left, right, t) : Vector3.Lerp(right, left, t));
                    curPos = _rb.position;

                    t += (Time.deltaTime * speed);
                }
                else _rb.position = curPos;

                yield return new WaitForEndOfFrame();
            }

            t = 0f;

            goingRight = !goingRight;

            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator Attack()
    {
        int _attack = 0;

        while (true)
        {
            yield return new WaitForSeconds(tickAttack);

            if (Vector3.Distance(transform.position, _player.transform.position) < attackeDistance)
            {
                if (_attack < 3)
                {
                    Barage();
                    _attack++;
                }
                else
                {
                    AllAround();
                    _attack = 0;
                }
            }
        }
    }

    protected void Barage()
    {
        for (int i = 0; i < barageMuzzles.Length; i++)
        {
            Instantiate(missileBarage, barageMuzzles[i].position, barageMuzzles[i].rotation);
        }
    }

    protected void AllAround()
    {
        for (int i = 0; i < allaroundMuzzles.Length; i++)
        {
            var _missile = Instantiate(missileAllAround).GetComponent<SeekingMissile>();
            _missile.DefineTarget(GetTarget());
            _missile.SpawnProjectile(allaroundMuzzles[i].position, allaroundMuzzles[i].transform.up);
        }
    }

    protected Transform GetTarget()
    {
        if (_isMindControlled)
        {
            EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

            List<EnemyBase> aimableEnemies = new List<EnemyBase>();

            int _i = 0;

            aimableEnemies.Add(this);

            if (enemies.Length > 1)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (Vector3.Distance(enemies[i].transform.position, transform.position) < attackeDistance && !enemies[i].Equals(this))
                    {
                        aimableEnemies.Add(enemies[i]);
                    }
                }

                _i = Random.Range(1, aimableEnemies.Count) - 1;
            }

            return aimableEnemies[_i].transform;
        }
        else
        {
            return _player.transform;
        }
    }

    protected override void MindControlHandler(bool state) { }

    protected override void Shoot() { }

    protected override void StunHandler(bool state) { }
}
