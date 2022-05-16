using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Controller : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer myRenderer;
    private BoxCollider2D playerCollider;
    [SerializeField]
    private Animator myAnimator;
    #region  variables
    [SerializeField]
    private float damageKnockback = 10f;
    private LayerMask damageLayer = 6;
    [SerializeField]
    private HealthbarUI healthbar;
    public int Lives { get; set; } = 5;
    #endregion
    #region X axis Movement variables
    [SerializeField] private GameObject parryBubble;
    private int activeFrames = 0;
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
    private void OnEnable()
    {
        if (SaveSystem.LoadPlayer() == null)
        {
            SaveState();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        //parryBubble.enabled = false;
        healthbar.SetMaxHealth(Lives);
        LoadState();
    }
    #region Loading and Saving
    /// <summary>
    /// Saves the current state of the player on a binary file 
    /// </summary>
    public void SaveState()
    {

        SaveSystem.SavePlayer(this);
    }
    /// <summary>
    /// Loads the player state stored in the SaveSystem class
    /// </summary>
    public void LoadState()
    {
        PlayerData playerInfo = SaveSystem.LoadPlayer();
        if (playerInfo != null)
        {
            Lives = playerInfo.PlayerLives;
            healthbar.SetHealth(Lives);
            this.transform.position = new Vector3(playerInfo.PlayerPosition[0], playerInfo.PlayerPosition[1], playerInfo.PlayerPosition[2]);
            Debug.Log("Player loaded from fun file");
        }
    }
    #endregion
    #region update
    // Update is called once per frame
    void Update()
    {
        horizontalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).x;
        if (activeFrames > 0)
            activeFrames--;
        if (Input.GetKeyDown(KeyCode.Z) && activeFrames <= 0)
        {
            activeFrames = 60;
            playerCollider.enabled = false;
            EnableBubble();
        }
        if (parryBubble.GetComponent<CircleCollider2D>().enabled == true && activeFrames <= 20)
        {
            playerCollider.enabled = true;
            DisableBubble();
        }
        CheckGrounded();
        Debug.Log(activeFrames);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == damageLayer)
        {
            ApplyKnockback(collision);
        }
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
    //Method that enables parryBubble and disables it after 10 frames
    private void EnableBubble()
    {
        if (!parryBubble.GetComponent<CircleCollider2D>().enabled)
        {
            parryBubble.GetComponent<CircleCollider2D>().enabled = true;
            parryBubble.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void DisableBubble()
    {
        parryBubble.GetComponent<CircleCollider2D>().enabled = false;
        parryBubble.GetComponent<SpriteRenderer>().enabled = false;
    }
    #region damage
    public void GetHurt(int hits)
    {
        myAnimator.SetTrigger("TakeDamage");
        Lives -= hits;
        if (Lives <= 0)
        {
            Lives = 5;
            Debug.Log("Player died");
            LoadState();
        }
        healthbar.SetHealth(Lives);
    }
    public void ApplyKnockback(Collision2D collision)
    {
        Vector2 force = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.y) * damageKnockback;
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
    #endregion
    #region horizontal movement
    /// <summary>
    /// Increases character horizontal speed by its acceleration value
    /// </summary>
    private void MoveCharacter()
    {
        rigidBody.AddForce(new Vector2(horizontalInput, 0f) * acceleration);
        if (horizontalInput != 0f)
        {
            myRenderer.flipX = horizontalInput < 0f;
        }


        if (Math.Abs(rigidBody.velocity.x) > maxSpeed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
        }
        myAnimator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
    }
    #endregion
    #region input
    /// <summary>
    /// Gets the user input for the x and y axis
    /// </summary>
    /// <returns></returns>

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
    /// This method is just for visualizing the raycast that detects collisions with the ground
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }
    #endregion
}