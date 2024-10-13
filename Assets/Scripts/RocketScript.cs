using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float speed;

    private bool isDestroyed;

    private KillTime timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        timer = GetComponent<KillTime>();

        StartCoroutine(timer.KillTimer(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

}