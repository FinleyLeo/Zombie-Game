using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;

    public GameObject bloodSplatter;

    private KillTime timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = GetComponent<KillTime>();

        StartCoroutine(timer.KillTimer(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
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
            Destroy(gameObject);
        }
    }

}
