using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperScript : MonoBehaviour
{
    LayerMask groundLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f;
        bool collided = false;

        Vector3 offset = new Vector3(xoffs, yoffs, 0);

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position + offset, Vector2.down, rayLength, groundLayerMask);

        Color hitColor = Color.white;

        if (hit.collider != null)
        {
            hitColor = Color.green;
            collided = true;
        }

        Debug.DrawRay(transform.position + offset, Vector2.down * rayLength, hitColor);

        return collided;
    }
}
