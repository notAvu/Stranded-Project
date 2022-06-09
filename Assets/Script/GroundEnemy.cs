using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float maxSpeed = 1.6f;
    private SpriteRenderer enemyRenderer;
    private Transform currentPosition;
    private float initialPositionY;
    private float initialPositionX;
    [SerializeField]
    private float finalPositionX;
    [SerializeField]
    public int Damage { get; set; } = 1;
    private int direction = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (collision.collider.gameObject.GetComponent<Controller>() != null)
            {
                collision.gameObject.GetComponentInChildren<GrappleGun>().Disbable();
                collision.collider.gameObject.GetComponent<Controller>().RecieveDamage(Damage);
                
            }
        }
    }
    public void Die()
    {
        enemyRenderer.enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void Respawn()
    {
        //set position to initial position
        this.transform.position = new Vector2(initialPositionX, initialPositionY);
        enemyRenderer.enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        currentPosition = transform;
        initialPositionX = currentPosition.position.x;
        initialPositionY = currentPosition.position.y;

    }
    private void Update()
    {
        if (enemyRenderer.enabled)
            Patrol();
    }
    /// <summary>
    /// Moves the enemy from its initial position to the final position and vice versa 
    /// TODO Improve this method to actually use physics instead of just a simple position change
    /// </summary>
    private void Patrol()
    {
        if (transform.position.x >= finalPositionX)
        {
            direction = -1;
        }
        else if (transform.position.x <= initialPositionX)
        {
            direction = 1;
        }
        enemyRenderer.flipX = direction == -1;
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);

    }

}
