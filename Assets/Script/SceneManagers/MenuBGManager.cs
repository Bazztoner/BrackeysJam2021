using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBGManager : MonoBehaviour
{
    [SerializeField]
    protected Transform planetarium;

    [SerializeField]
    protected float speed;

    void Update()
    {
        Quaternion rot = transform.rotation;
        Quaternion nextRot = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, Time.deltaTime * speed));

        transform.rotation = nextRot;
    }
}
