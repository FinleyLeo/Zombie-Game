using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Transform target;

    private GameObject player;
    public GameObject explosion, coin;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private AudioSource bite;
    public AudioSource deathSound;
    public AudioSource hit;

    private float speed = 3;
    public float maxHealth = 3;
    public float health;
    public float coolDown;

    private int coinQuantity;

    private bool isAttacking;
    private bool isColliding;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        bite = GetComponent<AudioSource>();

        player = GameObject.Find("Player");
        target = GameObject.Find("Player").transform;

        health = maxHealth;
        coinQuantity = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        coolDown -= Time.deltaTime;

        if (target != null && isAttacking == false && !isDead)
        {
            EnemyDirection();
            EnemyMovement();
        }
    }

    void EnemyDirection()
    {
        Vector3 enemyDir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(enemyDir.x, enemyDir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, -angle);
    }

    void EnemyMovement()
    {
        transform.position += speed * Time.deltaTime * transform.up;
    }

    void AttackStart()
    {
        isAttacking = true;
    }

    void Attack()
    {
        if (isColliding && !isDead)
        {
            bite.Play();
            player.GetComponent<PlayerController>().TakeDamage(1);
        }

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        hit.Play();
        CameraShake.Instance.ShakeCamera(0.5f, 0.25f);
        health -= damage;

        if (health <= 0)
        {
            foreach (Collider2D c in GetComponents<Collider2D>())
            {
                c.enabled = false;
            }

            for (int i = 0; i < coinQuantity; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);

                Instantiate(coin, transform.position + offset, Quaternion.identity);
            }

            deathSound.Play();
            CameraShake.Instance.ShakeCamera(2f, 0.25f);
            Instantiate(explosion, transform.position, Quaternion.identity);
            isDead = true;
            sr.enabled = false;
            StartCoroutine(KillTime());
        }
    }

    IEnumerator KillTime()
    {
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && coolDown <= 0 && !isDead)
        {
            anim.SetTrigger("Attack");
            coolDown = 1f;
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
}
