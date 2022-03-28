using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer myRenderer;

    #region X axis Movement variables
    [SerializeField] private CircleCollider2D parryBubble;
    [SerializeField] private float acceleration;      //Example value= 50f
    [SerializeField] private float maxSpeed;          //Example value= 12f
    [SerializeField] private float groundLinearDrag;  //Example value= 10f
    private float horizontalInput;
    private bool changinDirection => (rigidBody.velocity.x > 0f && horizontalInput < 0f) || (rigidBody.velocity.x < 0f && horizontalInput > 0f);
    #endregion
    #region jump variables
    [SerializeField] private float jumpForce;//Example value= 12f
    [SerializeField] private float fallMultiplier;//multiplier of gravity for character falling
    [SerializeField] private float upwardsMultiplier;//multiplier of gravity for character jumping (to make adjusting jump feel easier)
    //[SerializeField] private float lowJumpMultiplier/*=0.5f*/;
    [SerializeField] private float airLinearDrag/*=0.7f*/;
    #endregion
    #region check jump variables  
    [SerializeField] private LayerMask groundLayer;
    private float raycastLength = 1f;
    private bool grounded;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        parryBubble = GetComponent<CircleCollider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        //parryBubble.enabled = false;
    }
    #region update
    // Update is called once per frame
    void Update()
    {
        horizontalInput = GetInput().x;
        if (Input.GetKey(KeyCode.Z))
        {
            parryBubble.enabled = true;

        }
        else
        {
            parryBubble.enabled = false;
        }
        CheckGrounded();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetType() == typeof(CircleCollider2D) && collision.gameObject.tag.Equals("Enemies"))
        {
            //collision.
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        //if (otherCollider.tag.Equals("Enemies"))
        //{
        //}
    }
    private void FixedUpdate()
    {
        MoveCharacter();
        FallMultiplier();
        ApplyActualDrag();
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }
    }
    #endregion
    #region horizontal movement
    /// <summary>
    /// Increases character horizontal speed by its acceleration value
    /// </summary>
    private void MoveCharacter()
    {

        rigidBody.AddForce(new Vector2(horizontalInput, 0f) * acceleration);
        if (horizontalInput < 0)
        {
            myRenderer.flipX = true;
        }else if(horizontalInput > 0)
        {
            myRenderer.flipX = false;
        }

        if (Math.Abs(rigidBody.velocity.x) > maxSpeed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
        }
    }
    #endregion
    #region input
    /// <summary>
    /// Gets the user input for the x and y axis
    /// </summary>
    /// <returns></returns>
    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    #endregion
    #region drag
    /// <summary>
    /// Applies drag to decrease player's speed when not inputing any directions (Slight margin to start slowing player)
    /// </summary>
    void ApplyActualDrag()
    {
        if (grounded)
        {
            ApplyDrag(groundLinearDrag);
        }
        else
        {
            ApplyDrag(airLinearDrag);
        }
    }
    private void ApplyDrag(float dragValue)
    {
        if (Math.Abs(horizontalInput) < 0.4f || changinDirection)
        {
            rigidBody.drag = dragValue;
        }
        else
        {
            rigidBody.drag = 0f;
        }
    }
    #endregion
    #region jumping
    private void Jump()
    {
        ApplyActualDrag();
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    /// <summary>
    /// Applies forces to the falling part of the character's jump arc to make it feel less floaty
    /// </summary>
    private void FallMultiplier()
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = fallMultiplier;
        }
        //else if (rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) 
        //{
        //    rigidBody.gravityScale=lowJumpMultiplier;
        //}  
        else
        {
            rigidBody.gravityScale = upwardsMultiplier;
        }
    }
    /// <summary>
    /// Checks if there is ground within a set distance from the center of the player's rigibody
    /// </summary>
    private void CheckGrounded()
    {
        grounded = Physics2D.Raycast(transform.position * raycastLength, Vector2.down, raycastLength, groundLayer);

    }
    /// <summary>
    /// This method is just for visualizing the raycast
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }
    #endregion

}

