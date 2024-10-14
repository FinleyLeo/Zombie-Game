using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform rocket;

    public float speed;
    public float explodeRadius;

    public GameObject explosion;

    private bool isDestroyed;

    private KillTime timer;

    public LayerMask enemyLayers;

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
        if (!isDestroyed)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    void Explosion()
    {
        Instantiate(explosion, rocket.transform.position, Quaternion.identity);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rocket.transform.position, explodeRadius, enemyLayers);
        
        foreach(Collider2D e in hitEnemies)
        {
            e.GetComponent<EnemyScript>().TakeDamage(5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Explosion();
            CameraShake.Instance.ShakeCamera(4f, 0.5f);
            isDestroyed = true;
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rocket.transform.position, explodeRadius);
    }
}