using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private AudioSource audioSource;
    void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies")
        {
            collision.gameObject.GetComponent<GroundEnemy>().Die();
            Vector2 force = new Vector2(collision.GetContact(0).normal.x, collision.GetContact(0).normal.y) * 25;
            playerRb.AddForce(force, ForceMode2D.Impulse);
            audioSource.Play();
        }
    }
}
