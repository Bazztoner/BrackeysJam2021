using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlanetMarker : MonoBehaviour
{
    Animator _an;
    PlayerController _player;
    public float distanceToProc;
    float _d;
    bool _on;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _an = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!_on) CheckPlayerDistance();
    }

    void CheckPlayerDistance()
    {
        _d = Vector2.Distance(transform.position, _player.transform.position);
        if (_d <= distanceToProc)
        {
            _an.CrossFadeInFixedTime("FadeIn", .1f);
            _on = true;
        }
    }
}
