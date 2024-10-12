using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TDGP;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Stats/Floats
    public float maxHealth = 6, health;
    public float speed = 5, maxSpeed = 7;
    public float coins;
    public float lightCharge = 60f;

    // Inputs
    private float horizontalInput, verticalInput;

    // Booleans
    private bool isPlaying;
    private bool lightActive;
    private bool soundPlayed;
    public bool onFloor = true;
    public bool onGround = false;

    // Player Components
    private Rigidbody2D rb;
    private Collider2D col;

    // Audio Components
    public AudioSource audioSource;
    public AudioSource ambientAudio;
    public AudioSource floorWalking;
    public AudioSource grassWalking;
    public AudioClip coinPickup;
    public AudioClip Flashswitch;
    public AudioClip outsideAmb;
    public AudioClip indoorAmb;

    // GameObjects
    public GameObject cam, flashLight;

    // UI
    public Image battery;
    public Animator batteryAnim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        lightActive = false;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LightToggle();
        LightCharge();
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
        if (rb.velocity.magnitude > 0 && !isPlaying && onFloor)
        {
            floorWalking.Play();
            isPlaying = true;
        }

        if (rb.velocity.magnitude > 0 && !isPlaying && onGround)
        {
            grassWalking.Play();
            isPlaying = true;
        }

        if (rb.velocity.magnitude <= 0)
        {
            floorWalking.Stop();
            grassWalking.Stop();
            isPlaying = false;
        }
    }

    // Toggles FlashLight
    void LightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F) && !soundPlayed)
        {
            audioSource.PlayOneShot(Flashswitch);
            lightActive = !lightActive;
        }

        if (lightActive == true && lightCharge > 0)
        {
            flashLight.SetActive(true);

            if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize < 8)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize += 4f * Time.deltaTime;

            }

            else if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize > 8)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 8;
            }

            lightCharge -= Time.deltaTime;
        }

        else if (lightActive == false || lightCharge <= 0)
        {
            flashLight.SetActive(false);

            if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize > 6)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize -= 4f * Time.deltaTime;
            }

            else if (cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize < 6)
            {
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6;
            }
        }

        if (lightCharge <= 0  && !soundPlayed && lightActive)
        {
            audioSource.PlayOneShot(Flashswitch);
            soundPlayed = true;
        }

        else if (lightCharge > 0)
        {
            soundPlayed = false;
        }
    }

    void LightCharge()
    {
        batteryAnim.SetFloat("Charge", lightCharge);
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

        if (collision.gameObject.CompareTag("Floor") && collision.IsTouching(col))
        {
            grassWalking.Stop();
            floorWalking.Play();
            onGround = false;
            onFloor = true;
            ambientAudio.Stop();
            ambientAudio.PlayOneShot(indoorAmb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            floorWalking.Stop();
            grassWalking.Play();
            onFloor = false;
            onGround = true;
            ambientAudio.Stop();
            ambientAudio.PlayOneShot(outsideAmb);
    }
    }
}
