using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TDGP;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Stats/Floats
    public float maxHealth = 6, health;
    public float speed = 5, maxSpeed = 7;
    public float coins;

    // Inputs
    private float horizontalInput, verticalInput;

    // Booleans
    private bool isPlaying;
    private bool lightActive;

    // Player Components
    private Rigidbody2D rb;
    private Collider2D col;

    // Audio Components
    public AudioSource audioSource;
    public AudioSource walking;
    public AudioClip coinPickup;
    public AudioClip Flashswitch;

    // GameObjects
    public GameObject cam, flashLight;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        lightActive = false;
        health = maxHealth;

        Physics2D.IgnoreCollision(GameObject.FindWithTag("Entrance").GetComponent<Collider2D>(), col);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LightToggle();
    }

    // Player Movement in all directions
    void Movement()
    {
        // Gets Vertical and Horizontal Inputs (WASD/Arrow Keys)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Moves player in direction of inputs
        rb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        // Stops Speed Increases when using diagonal inputs (like W and D together)
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        // Plays footstep walking sounds when moving
        if (rb.velocity.magnitude > 0 && !isPlaying)
        {
            walking.Play();
            isPlaying = true;
        }

        else if (rb.velocity.magnitude <= 0)
        {
            walking.Stop();
            isPlaying = false;
        }
    }

    // Toggles FlashLight
    void LightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(Flashswitch);
            lightActive = !lightActive;
        }

        if (lightActive == true)
        {
            if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize < 8)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize += 4f * Time.deltaTime;
            }

            else if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize > 8)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 8;
            }
        }

        else if (lightActive == false)
        {
            if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize > 6)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize -= 4f * Time.deltaTime;
            }

            else if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize < 6)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6;
            }
        }

        flashLight.SetActive(lightActive);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        HealthCheck();

        if (health <= 0)
        {
            print("You died");
            Destroy(gameObject);
        }
    }

    // Checks for health value (will be used later for effects like heartbeat)
    void HealthCheck()
    {
        if (health < 6 && health >= 4)
        {
            print("you are slightly hurt, " + health + " health left");
        }

        else if (health < 4 && health >= 2)
        {
            print("you are very hurt, " + health + " health left");
        }

        else if (health < 2 && health > 0)
        {
            print("you are close to death, " + health + " health left");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin") && collision.IsTouching(col))
        {
            audioSource.PlayOneShot(coinPickup);
            coins++;
            Destroy(collision.gameObject);
        }
    }
}
