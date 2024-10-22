using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private EnemyScript script;

    public float dashSpeed = 20;

    public bool dashing;

    public GameObject zombie;
    public GameObject[] spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        script = GetComponent<EnemyScript>();
        dashing = true;
        dashing = false;
        InvokeRepeating("BossAttack", 3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing)
        {
            transform.position += 20 * Time.deltaTime * transform.up;
        }
    }

    void BossAttack()
    {
        int randomAttack = Random.Range(1, 3);

        if (randomAttack == 1)
        {
            script.isAttacking = true;

            StartCoroutine(DashAttack());
        }

        else if (randomAttack == 2)
        {
            SpawnAttack();
        }
    }

    IEnumerator DashAttack()
    {
        yield return new WaitForSeconds(0.5f);

        dashing = true;

        yield return new WaitForSeconds(0.5f);

        dashing = false;
        script.isAttacking = false;
    }

    void SpawnAttack()
    {
        Instantiate(zombie, spawnPos[0].transform.position, Quaternion.identity);
        Instantiate(zombie, spawnPos[1].transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && dashing)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(4);
        }
    }
}
