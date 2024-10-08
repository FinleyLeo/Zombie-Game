using System.Collections;
using System.Collections.Generic;
using TDGP;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float maxSpeed = 7;

    private float horizontalInput;
    private float verticalInput;

    public float maxHealth = 6, health;

    public bool isGrounded;
    private bool isPlaying;

    private Rigidbody2D rb;
    private HelperScript helper;

    LayerMask groundLayerMask;

    public AudioSource audioSource;
    public AudioClip walking;

    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        helper = GetComponent<HelperScript>();

        groundLayerMask = LayerMask.GetMask("Ground");

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CameraMovement();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        rb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (rb.velocity.magnitude > 0 && !isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }

        else if (rb.velocity.magnitude <= 0)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    void CameraMovement()
    {
        if (transform.position.y > 5)
        {
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }

        else if (transform.position.y < 5)
        {
            cam.transform.position = new Vector3(transform.position.x, -1.5f, -10);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
