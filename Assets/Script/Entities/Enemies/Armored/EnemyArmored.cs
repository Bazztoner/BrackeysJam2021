using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmored : EnemyBase
{
    [SerializeField]
    protected float maxDistance, detectDistance, tickShoot, rotateSpeed;

    Coroutine shoot, aim, movement;

    protected override void Start()
    {
        base.Start();

        aim = StartCoroutine(Aim());
    }

    IEnumerator Movement()
    {
        if (aim != null) StopCoroutine(aim);

        float t = 0f;

        Vector3 posInit = transform.position;
        Vector3 posEnd = transform.position + (transform.up * maxDistance);

        while (t < 1f && !_isStunned)
        {
            _rb.MovePosition(Vector3.Lerp(posInit, posEnd, t));

            t += Time.fixedDeltaTime * movementSpeed;

            yield return new WaitForEndOfFrame();
        }

        shoot = StartCoroutine(TakeShot());
    }

    IEnumerator Aim()
    {
        if (shoot != null) StopCoroutine(shoot);

        float t = 0f;

        Vector3 rotInit = transform.up;
        Vector3 rotEnd = Vector3.Normalize(GetTarget() - transform.position);

        while (t < 1f)
        {
            transform.up = Vector3.Lerp(rotInit, rotEnd, t);

            t += Time.fixedDeltaTime * rotateSpeed;

            yield return new WaitForEndOfFrame();
        }

        movement = StartCoroutine(Movement());
    }

    protected Vector3 GetTarget()
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
                    if (Vector3.Distance(enemies[i].transform.position, transform.position) < detectDistance && !enemies[i].Equals(this))
                    {
                        aimableEnemies.Add(enemies[i]);
                    }
                }

                _i = Random.Range(1, aimableEnemies.Count) - 1;
            }

            return aimableEnemies[_i].transform.position;
        }
        else
        {
            return _player.transform.position;
        }
    }

    IEnumerator TakeShot()
    {
        if (movement != null) StopCoroutine(movement);

        yield return new WaitForSeconds(tickShoot);

        Shoot();

        yield return new WaitForSeconds(tickShoot);

        aim = StartCoroutine(Aim());
    }

    protected override void MindControlHandler(bool state)
    {
    }

    protected override void Shoot()
    {
        var projectile = Instantiate(defaultProjectile, muzzle.position, muzzle.rotation);
        projectile.SpawnProjectile(muzzle.position, transform.up, this);
    }

    protected override void StunHandler(bool state)
    {
        //Inmune
    }
}
