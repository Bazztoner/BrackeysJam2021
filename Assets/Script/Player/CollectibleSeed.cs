using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSeed : MonoBehaviour
{
    [SerializeField]
    SeedTypes thisSeed;

    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer rend;

    [SerializeField]
    float speed;

    Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        rend = GetComponentInChildren<SpriteRenderer>();
        rend.sprite = sprites[(int)thisSeed];

        _rb.velocity = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized * speed;
    }

    public void SetSeed(int i)
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = sprites[i];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.LayerMatchesWith("Player"))
        {
            GameManager.Instance.GainSeeds((int)thisSeed);
            Destroy(gameObject);
        }
    }
}
