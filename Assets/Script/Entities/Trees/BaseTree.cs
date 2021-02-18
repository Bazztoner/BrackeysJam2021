using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTree : MonoBehaviour
{
    public Collider2D col;

    public virtual void Sprout()
    {
        StartCoroutine(Sprouting());
    }

    IEnumerator Sprouting()
    {
        //Sprouting animation?

        yield return new WaitForEndOfFrame();

        col.enabled = true;
    }
}
