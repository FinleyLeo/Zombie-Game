using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Transform target;

    private GameObject player;

    private Rigidbody2D rb;
    private Animator anim;


    private float speed = 3;
    public float maxHealth = 3;
    public float health;
    public float coolDown;

    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        player = GameObject.Find("Player");
        target = GameObject.Find("Player").transform;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        coolDown -= Time.deltaTime;

        if (target != null && isAttacking == false)
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
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void AttackStart()
    {
        isAttacking = true;
    }

    void Attack()
    {
        player.GetComponent<PlayerController>().TakeDamage(1);

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && coolDown <= 0)
        {
            anim.SetTrigger("Attack");
            coolDown = 1f;
        }
    }
}
