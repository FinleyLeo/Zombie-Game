using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float speed;

    private bool isDestroyed;

    public GameObject bloodSplatter;

    private KillTime timer;

    public AudioSource hitWall;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyScript>().TakeDamage(1);
            Instantiate(bloodSplatter, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            hitWall.Play();
            CameraShake.Instance.ShakeCamera(0.5f, 0.25f);
            sr.enabled = false;
            isDestroyed = true;
        }
    }

}
