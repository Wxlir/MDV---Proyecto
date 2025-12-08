
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

    [Header("Health System")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    
    [Header("UI References")]
    [SerializeField] private HealthBar healthBar; // Referencia a la barra de vida

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        
        // Save initial position (the position set in the scene)
        initialPosition = transform.position;

        // Inicializar vida
        currentHealth = maxHealth;
        
        // Actualizar barra de vida inicial
        if (healthBar != null)
        {
            healthBar.SetHealthInstant(currentHealth, maxHealth);
        }
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
        // Si está muerto, no hacer nada
        if (isDead)
            return;

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

    #region Health System

    /// <summary>
    /// Hace daño al jugador
    /// </summary>
    /// <param name="damage">Cantidad de daño a recibir</param>
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}/{maxHealth}");
        
        // Actualizar barra de vida
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }

        // Aquí puedes agregar efectos visuales, sonidos, etc.
        // Por ejemplo: animación de daño, sonido de dolor

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Cura al jugador
    /// </summary>
    /// <param name="amount">Cantidad de vida a recuperar</param>
    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player healed {amount}. Current health: {currentHealth}/{maxHealth}");
        
        // Actualizar barra de vida
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador
    /// </summary>
    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");

        // Detener movimiento
        rb.linearVelocity = Vector2.zero;

        // Aquí puedes agregar:
        // - Animación de muerte
        // - Desactivar controles
        // - Mostrar pantalla de Game Over
        // - Reproducir sonido de muerte

        // Ejemplo: anim.SetTrigger("Death");
    }

    /// <summary>
    /// Revive al jugador (útil para respawn)
    /// </summary>
    public void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        Debug.Log("Player respawned!");
    }

    /// <summary>
    /// Devuelve la vida actual del jugador
    /// </summary>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Devuelve la vida máxima del jugador
    /// </summary>
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Devuelve si el jugador está muerto
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }

    #endregion

}
