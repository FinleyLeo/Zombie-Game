using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private GameObject player;
    private Collider2D col;

    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin") && collision.IsTouching(col))
        {
            collision.transform.position = Vector2.MoveTowards(collision.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
