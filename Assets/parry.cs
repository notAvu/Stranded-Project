using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    private Rigidbody2D playerRb;
    void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies")
        {
            collision.gameObject.GetComponent<GroundEnemy>().Die();
            Vector2 force = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.y) * 15;
            playerRb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
