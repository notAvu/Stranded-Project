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
    [SerializeField]
    private LayerMask damageLayer = 6;
    [SerializeField]
    private HealthbarUI healthbar;
    [SerializeField]
    private int maxHealth = 5;
    [SerializeField]
    private float invulSeconds = 1f;
    private bool isInvulnerable;
    public int Lives { get; set; } = 5;
    #endregion
    #region X axis Movement variables
    [SerializeField] private GameObject parryBubble;
    private float activeFrames = 0;
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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
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
        ExecuteParry();
        CheckGrounded();
    }
    /// <summary>
    /// This methods checks if the player is able to parry and is inputing the parry button, if so enables the parry bubble.
    /// Once active the parry bubble will be disabled after a set amount of time.
    /// 
    /// -- TODO : Set parry enabling/disabling to a corroutine -- 
    /// </summary>
    private void ExecuteParry()
    {
        if (activeFrames > 0f)
            activeFrames--;
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.DownArrow)) && activeFrames <= 0f)
        {
            activeFrames = 60f;
            playerCollider.enabled = false;
            EnableBubble();
        }
        if (parryBubble.GetComponent<CircleCollider2D>().enabled && activeFrames <= 20f)
        {
            playerCollider.enabled = true;
            DisableBubble();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Checkpoint"))
            SaveState();
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
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && grounded)
        {
            Jump();
        }
    }

    #endregion
    /// <summary>
    /// If parry bubble is not enabled it enalbles it and makes it visible.
    /// </summary>
    private void EnableBubble()
    {
        if (!parryBubble.GetComponent<CircleCollider2D>().enabled)
        {
            parryBubble.GetComponent<CircleCollider2D>().enabled = true;
            parryBubble.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    /// <summary>
    /// disables the parry bubble and makes it invisible.
    /// </summary>
    private void DisableBubble()
    {
        parryBubble.GetComponent<CircleCollider2D>().enabled = false;
        parryBubble.GetComponent<SpriteRenderer>().enabled = false;
    }
    #region damage
    /// <summary>
    /// This method triggers the animation for taking damage. Then reduces the player's health by the hits parameter
    /// and checks if life has reached zero, and if it did it loads the last saved state and sets the number of 
    /// layers to the player's starting lives.
    /// </summary>
    /// <param name="hits"></param>
    public void GetHurt(int hits)
    {
        if (!isInvulnerable)
        {
            myAnimator.SetTrigger("TakeDamage");
            Lives -= hits;
            if (Lives <= 0)
            {
                Lives = maxHealth;
                LoadState();
            }
            StartCoroutine(BecomeInbulnerable());
            healthbar.SetHealth(Lives);
        }
    }
    /// <summary>
    /// This method pushes the player back after being hit.
    /// The force applied is based on the direction of the collision.
    /// </summary>
    /// <param name="collision"></param>
    public void ApplyKnockback(Collision2D collision)
    {
        Vector2 force = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.y) * damageKnockback;
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
    /// <summary>
    /// this method makes the player invulnerable for a set amount of time .
    /// </summary>
    /// <returns></returns>
    private IEnumerator BecomeInbulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulSeconds);
        isInvulnerable = false;
    }
    #endregion
    #region horizontal movement
    /// <summary>
    /// Increases character horizontal speed by its acceleration value up to the value of maxSpeed.
    /// The player's sprite is flipped depending on the direction of movement.
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
    /// <summary>
    /// Applies a given drag to the player's rigidbody if it's .
    /// </summary>
    /// <param name="dragValue"></param>
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

    /// <summary>
    /// Pushes the player upwards to make it jump andtriggers the jump animation
    /// </summary>
    private void Jump()
    {
        ApplyActualDrag();
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        myAnimator.SetTrigger("Jump");
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