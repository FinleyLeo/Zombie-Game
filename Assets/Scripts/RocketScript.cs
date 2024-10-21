using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    [Header("Components")]
    public Transform rocket;
    public GameObject explosion;
    private KillTime timer;
    public AudioSource missileFly;

    [Header("Variables")]
    public float speed;
    public float explodeRadius;
    private bool isDestroyed;

    [Header("LayerMask")]
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<KillTime>();

        missileFly.Play();

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
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rocket.transform.position, explodeRadius);
    }
}