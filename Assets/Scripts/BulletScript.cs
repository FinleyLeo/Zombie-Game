using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Components")]
    private SpriteRenderer sr;
    private PlayerController script;
    private PlayerAimWeapon playerAim;
    public AudioSource hitWall;
    private KillTime timer;

    [Header("Variables")]
    public float speed;
    private bool isDestroyed;

    [Header("GameObjects")]
    public GameObject player;
    public GameObject bloodSplatter;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerController>();
        playerAim = player.GetComponent<PlayerAimWeapon>();

        timer = GetComponent<KillTime>();

        StartCoroutine(timer.KillTimer(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (script.currentGun == CurrentGun.sniper)
        {
            speed = 100;
        }

        if (!isDestroyed)
        {
            transform.position += speed * Time.deltaTime * transform.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyScript>().TakeDamage(playerAim.damage);
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
