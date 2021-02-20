using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    protected float laserDamage, maxLength, maxPosition, tickExtension, initLength;

    [SerializeField]
    protected SpriteRenderer[] laserLayers;

    Vector3 _lengthInit;
    Vector3 _lengthEnd;

    Vector3 _posInit;
    Vector3 _posEnd;

    protected Coroutine shoot;

    public void Initialize()
    {
        _lengthInit = new Vector3(transform.localScale.x, initLength, transform.localScale.z);
        _lengthEnd = new Vector3(_lengthInit.x, maxLength, _lengthInit.z);

        _posInit = new Vector3(0f, 0f, 0f);
        _posEnd = new Vector3(0f, maxPosition, 0f);
    }

    public void Shoot()
    {
        for (int i = 0; i < laserLayers.Length; i++)
        {
            laserLayers[i].enabled = true;
        }

        shoot = StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        float t = 0f;

        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(_posInit, _posEnd, t);
            transform.localScale = Vector3.Lerp(_lengthInit, _lengthEnd, t);

            yield return new WaitForEndOfFrame();

            t += (Time.fixedDeltaTime / tickExtension);
        }
    }

    public void EndLaser()
    {
        for (int i = 0; i < laserLayers.Length; i++)
        {
            laserLayers[i].enabled = false;
        }

        transform.localPosition = _posInit;
        transform.localScale = _lengthInit;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(laserDamage);
        }
    }
}
