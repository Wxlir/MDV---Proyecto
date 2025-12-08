
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;


    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 5;
    private float xInput;
    private bool facingRight = true;

    public bool GetFacingRight() => facingRight;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    private Vector3 initialPosition;
    private bool initialPositionSaved = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        
        // Save initial position (the position set in the scene)
        initialPosition = transform.position;
    }

    private void Start()
    {
        // Ensure GameManager exists
        GameManager.GetInstance();

        // Check if this is a new game (should reset to initial position)
        if (UnityEngine.PlayerPrefs.GetInt("NewGame", 0) == 1)
        {
            ResetToInitialPosition();
            UnityEngine.PlayerPrefs.SetInt("NewGame", 0);
            UnityEngine.PlayerPrefs.Save();
        }
        // Check if we need to load a saved game
        else if (UnityEngine.PlayerPrefs.GetInt("LoadGame", 0) == 1)
        {
            LoadGameState();
            UnityEngine.PlayerPrefs.SetInt("LoadGame", 0);
            UnityEngine.PlayerPrefs.Save();
        }
        // If we're returning from menu after saving, restore position from PlayerPrefs
        else if (UnityEngine.PlayerPrefs.HasKey("LastPlayerPosX"))
        {
            RestorePositionFromPlayerPrefs();
        }
    }

    /// <summary>
    /// Resets the player to the initial position (start of level)
    /// </summary>
    private void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        facingRight = true; // Reset facing direction to default
        
        // Reset rotation if needed
        transform.rotation = Quaternion.identity;
        
        // Reset velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        Debug.Log($"Player reset to initial position: {initialPosition}");
    }

    /// <summary>
    /// Restores player position from PlayerPrefs (used when returning from menu after saving)
    /// </summary>
    private void RestorePositionFromPlayerPrefs()
    {
        float posX = UnityEngine.PlayerPrefs.GetFloat("LastPlayerPosX");
        float posY = UnityEngine.PlayerPrefs.GetFloat("LastPlayerPosY");
        float posZ = UnityEngine.PlayerPrefs.GetFloat("LastPlayerPosZ");
        bool savedFacingRight = UnityEngine.PlayerPrefs.GetInt("LastPlayerFacingRight", 1) == 1;
        
        transform.position = new Vector3(posX, posY, posZ);
        
        // Restore facing direction
        if (savedFacingRight != facingRight)
        {
            Flip();
        }
        
        Debug.Log("Player position restored from PlayerPrefs");
    }

    /// <summary>
    /// Loads the saved game state
    /// </summary>
    private void LoadGameState()
    {
        SaveSystem.SaveData saveData = SaveSystem.LoadGame();
        if (saveData != null)
        {
            // Restore player position
            transform.position = new Vector3(saveData.playerPositionX, saveData.playerPositionY, saveData.playerPositionZ);
            
            // Restore facing direction
            if (saveData.facingRight != facingRight)
            {
                Flip();
            }

            Debug.Log("Game state loaded successfully!");
        }
    }


    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }



    private void HandleAnimations()
    {

        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);


    }


    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }


    private void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
            Flip();
        else if (rb.linearVelocity.x < 0 && facingRight == true)
            Flip();
    }


    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }


}
