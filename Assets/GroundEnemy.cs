using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField]
    private GrappleGun grapple;
    private Rigidbody2D rb;
    private float maxSpeed=1.6f;
    
    private Transform currentPosition;
    private float initialPositionX;
    private float initialPositionY;
    [SerializeField]
    private float finalPositionX;
    [SerializeField]
    private float finalPositionY;
    [SerializeField]
    public int Damage { get; set; } = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.gameObject.GetComponent<Controller>().GetHurt(Damage);
            grapple.Disbable();
        }
    }
    private void Start()
    {
        currentPosition = transform;
        initialPositionX = currentPosition.position.x;
        initialPositionY = currentPosition.position.y;

    }
    private void Update()
    {
        Patrol();
    }
    int direction=1;
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
        Move(direction);
    }

    private void Flip()
    {
        //
    }

    private void Move(int direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
    }

}
