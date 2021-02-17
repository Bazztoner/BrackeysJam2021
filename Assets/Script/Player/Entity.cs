using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Entity : MonoBehaviour
{
    public abstract void TakeDamage(float dmg);
}