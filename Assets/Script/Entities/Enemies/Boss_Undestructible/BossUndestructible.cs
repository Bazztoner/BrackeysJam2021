using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUndestructible : EnemyBase
{
    [SerializeField]
    protected CircleCollider2D col;

    [SerializeField]
    protected Transform pointFromLaser, pointLaserMax, pointLaserMin, shield;

    [SerializeField]
    protected Transform[] pointMuzzles;

    [SerializeField]
    protected MeshRenderer shields;

    [SerializeField]
    protected Color down, blink1, blink2;

    [SerializeField]
    protected float distanceToShield, distanceToLaser, tickBetweenAttackas, laserMaxAngle, laserSwipeSpeed, tickRiseShield;

    [SerializeField]
    protected Laser _laser;

    Coroutine idle, shielded, missiles, laser;

    bool isShielded;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToShield);
        Gizmos.DrawWireSphere(transform.position, distanceToLaser);
    }

    protected override void Start()
    {
        shields.material.color = down;
        shield.gameObject.SetActive(false);

        isShielded = false;

        base.Start();

        _laser.Initialize();

        idle = StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        if (shielded != null) StopCoroutine(shielded);
        if (missiles != null) StopCoroutine(missiles);
        if (laser != null) StopCoroutine(laser);

        float curDistance = Vector3.Distance(_player.transform.position, transform.position);

        Vector3 dirInit = transform.up;
        Vector3 dirEnd = Vector3.Normalize(_player.transform.position - transform.position);

        float t = 0f;

        while (t < 1f)
        {
            transform.up = Vector3.Lerp(dirInit, dirEnd, t);

            yield return new WaitForEndOfFrame();

            t += (Time.deltaTime / tickBetweenAttackas);
        }

        if (curDistance > distanceToLaser)
            laser = StartCoroutine(Laser());
        else if (curDistance < distanceToShield)
            shielded = StartCoroutine(Shields());
        else missiles = StartCoroutine(Missiles());

    }

    protected IEnumerator Laser()
    {
        _laser.Shoot();

        yield return new WaitForSeconds(.25f);

        float t = 0f;

        Vector3 initDir = transform.up;
        Vector3 endDir = pointLaserMax.up;

        while (t < 1f)
        {
            pointFromLaser.up = Vector3.Lerp(initDir, endDir, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime / (laserSwipeSpeed * 2f);
        }

        yield return new WaitForSeconds(.25f);

        t = 0f;

        initDir = pointLaserMax.up;
        endDir = pointLaserMin.up;

        while (t < 1f)
        {
            pointFromLaser.up = Vector3.Lerp(initDir, endDir, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime / (laserSwipeSpeed);
        }

        yield return new WaitForSeconds(.25f);

        t = 0f;

        initDir = pointLaserMin.up;
        endDir = pointLaserMax.up;

        while (t < 1f)
        {
            pointFromLaser.up = Vector3.Lerp(initDir, endDir, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime / (laserSwipeSpeed);
        }

        yield return new WaitForSeconds(.25f);

        t = 0f;

        initDir = pointLaserMax.up;
        endDir = pointLaserMin.up;

        while (t < 1f)
        {
            pointFromLaser.up = Vector3.Lerp(initDir, endDir, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime / (laserSwipeSpeed);
        }

        yield return new WaitForSeconds(.25f);

        t = 0f;

        initDir = pointLaserMin.up;
        endDir = transform.up;

        while (t < 1f)
        {
            pointFromLaser.up = Vector3.Lerp(initDir, endDir, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime / (laserSwipeSpeed * 2f);
        }

        yield return new WaitForSeconds(.25f);

        _laser.EndLaser();

        idle = StartCoroutine(Idle());
    }

    protected IEnumerator Shields()
    {
        shield.localScale = Vector3.zero;

        shield.gameObject.SetActive(true);

        col.enabled = false;

        float t = 0f;

        while (t < 1f)
        {
            shield.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            shields.material.color = Color.Lerp(down, blink1, t);

            yield return new WaitForEndOfFrame();

            t += (Time.deltaTime * tickRiseShield);
        }

        while (Vector3.Distance(_player.transform.position, transform.position) < distanceToShield)
        {
            t = 0f;

            while (t < 1f)
            {
                shields.material.color = Color.Lerp(blink1, blink2, t);

                yield return new WaitForEndOfFrame();

                t += (Time.deltaTime / tickRiseShield);
            }

            if (Vector3.Distance(_player.transform.position, transform.position) > distanceToShield)
                break;

            t = 0f;

            while (t < 1f)
            {
                shields.material.color = Color.Lerp(blink2, blink1, t);

                yield return new WaitForEndOfFrame();

                t += (Time.deltaTime / tickRiseShield);
            }

            yield return new WaitForEndOfFrame();
        }

        t = 0f;

        Color _cur = shields.material.color;

        while (t < 1f)
        {
            shield.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            shields.material.color = Color.Lerp(_cur, down, t);

            yield return new WaitForEndOfFrame();

            t += (Time.deltaTime * tickRiseShield);
        }

        col.enabled = true;

        idle = StartCoroutine(Idle());
    }

    protected IEnumerator Missiles()
    {
        yield return new WaitForSeconds(.25f);

        int _i = 0;

        for (int i = 0; i < 100; i++)
        {
            _i = Random.Range(0, pointMuzzles.Length - 1);

            var _missie = Instantiate(defaultProjectile, pointMuzzles[_i].position, pointMuzzles[_i].rotation);
            SeekingMissile missie = _missie.GetComponent<SeekingMissile>();
            missie.DefineTarget(_player.transform);

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.25f);

        idle = StartCoroutine(Idle());
    }

    protected override void MindControlHandler(bool state)
    {
        throw new System.NotImplementedException();
    }

    protected override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    protected override void StunHandler(bool state)
    {
        throw new System.NotImplementedException();
    }
}
