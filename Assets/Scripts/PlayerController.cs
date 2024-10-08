using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 10;
    public float maxSpeed = 7;

    private float horizontalInput;
    private float verticalInput;

    public bool isGrounded;

    private Rigidbody2D rb;
    private HelperScript helper;

    LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        helper = GetComponent<HelperScript>();

        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
    }

    public void RayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f;

        Vector3 offset = new Vector3(xoffs, yoffs, 0); 

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayerMask);

        Color hitColor = Color.white;
        isGrounded = false;

        if (hit.collider != null)
        {
            hitColor = Color.green;
            isGrounded = true;
        }

        Debug.DrawRay(transform.position, Vector2.down * rayLength, hitColor);
    }

}
