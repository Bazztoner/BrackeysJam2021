using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlanetOrbit : MonoBehaviour
{
    [SerializeField]
    protected Transform planet, player;

    public UnityEvent onEnterOrbit;

    IEnumerator EnteringOrbit()
    {
        Vector3 initPos = player.position;

        bool called = false;

        float t = 0f;

        while (t < 1f)
        {
            player.position = Vector3.Lerp(initPos, planet.position, t);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;

            if ((t > .25f) && !called)
            {
                onEnterOrbit.Invoke();
                called = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player")) 
            StartCoroutine(EnteringOrbit());
    }
}
