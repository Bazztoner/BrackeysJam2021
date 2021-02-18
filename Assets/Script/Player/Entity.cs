using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(FeedbackManager))]
public abstract class Entity : MonoBehaviour
{
    protected FeedbackManager fbMan;

    public abstract void TakeDamage(float dmg);

    public abstract void TakeHeal(float hp);
}