using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    // Public Fields
    public float speed = 1;

    // Private Fields
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] Collider2D standingCollider, crouchingCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float jumpPower = 10;
    [SerializeField] int totalJumps;

    [Header("Damage Settings")]
    [SerializeField] float knockbackForce = 8f;
    [SerializeField] float knockbackUpForce = 5f;
    [SerializeField] float hitStunDuration = 0.2f;
    [SerializeField] float flickerInterval = 0.1f;

    private SpriteRenderer spriteRenderer;
    private bool isHit = false;

    int availableJumps;
    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    float horizontalValue;
    float RunSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;
    public int extraLives = 0;
    private Vector3 lastDeathPosition;
    public float invincibleDuration = 2f;
    private Vector2 knockbackVelocity;

    [SerializeField] bool isGrounded = false;
    bool isRunning = false;
    bool facingRight = true;
    [SerializeField] bool crouchPressed = false;
    bool multipleJumps = false;
    bool coyoteJump = false;
    bool isDead = false;
    private bool isInvincible = false;

    void Awake()
    {
        availableJumps = totalJumps;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        // Check if the GroundCheck object is colliding
        // with other 2D Colliders that are in the ground layer
        // If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) // Grounded
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
                AudioManager.instance.SFX("landing");
            }

            // Check if any of the colliders is moving platform
            // Parent it to this transform
            foreach (var c in colliders)
            {
                if (c.tag == "MovingPlatform")
                transform.parent = c.transform;
            }
        }
        else
        {
            // Un-parent the transform
            transform.parent = null;

            if(wasGrounded)
            {
                coyoteJump = true;
                StartCoroutine(CoyoteJumpDelay());
            }
        }
    }

    IEnumerator CoyoteJumpDelay()
    {
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

        // Update is called once per frame
        void Update()
    {
         if (CanMove() == false)
            return;
        // Store the horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        // Set the yVelocity in the Animator
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        // If left shift is clicked, enable isrunning
        // If left shift is released, disable isrunning
        if (Input.GetKeyDown(KeyCode.LeftShift)) isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) isRunning = false;

        animator.SetBool("IsRunning", horizontalValue != 0 && isGrounded && !isHit);
        if (horizontalValue != 0)
        {
            if (isRunning)
            {
                animator.speed = 1.2f; // Normal/quicker for running
            }
            else
            {
                animator.speed = 0.6f; // Slow speed imitating walking
            }
        }
        else
        {
            animator.speed = 1f; // Idle or if stopped
        }
        // If we press jump button, enable it
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // If we press crouch button, enable it
        if (Input.GetButtonDown("Crouch"))
        { 
            crouchPressed = true; 
        }
        // Otherwise, disable it
        else if (Input.GetButtonUp("Crouch"))
        { 
            crouchPressed = false; 
        
        }
    }

    // Happens every fixed frame (for physics based interactions)
    void FixedUpdate()
    {
        GroundCheck();
        animator.SetBool("Jump", !isGrounded);

        Move(horizontalValue, crouchPressed);
    }

    public bool CanMove()
    {
        bool can = true;
        if (isDead)
            can = false;
        if (FindFirstObjectByType<InteractionSystem>().isExamining)
            can = false;
        if (FindFirstObjectByType<InventorySystem>().isOpen)
            can = false;
        if (isHit)
            can = false;
            return can;
    }

    public void OnJumpEnd()
    {
        animator.SetBool("Jump", false);
    }

    void Jump()
    {   
       if (isGrounded)
       {
         multipleJumps = true;
            availableJumps--;

         rb.linearVelocity = Vector2.up * jumpPower;
         AudioManager.instance.SFX("jumping");
        }
       else
        {
            if (coyoteJump)
            {
                availableJumps--;

                rb.linearVelocity = Vector2.up * jumpPower;
                AudioManager.instance.SFX("jumping");
            }

            if (multipleJumps && availableJumps > 0)
            {
                multipleJumps = true;
                availableJumps--;

                rb.linearVelocity = Vector2.up * jumpPower;
                AudioManager.instance.SFX("jumping");
            }
        }
    }

    void Move(float dir, bool crouchFlag)
    {
        #region Crouch
        // If we are crouching and disabled crouching
        // Check overheard for collisions with ground items 
        // If there are any, remain crouched, otherwise, uncrouch
        if (!crouchFlag)
        {
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
            {
                crouchFlag = true;
            }
        }

        animator.SetBool("IsCrouching", crouchFlag);
        standingCollider.enabled = !crouchFlag;
        crouchingCollider.enabled = crouchFlag;

        #endregion

        #region Move & Run
        // Set value of X using dir and speed
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        //If we are running, multiply with RunSpeedModifier
        if(isRunning) { xVal *= RunSpeedModifier ; }
        //If we are running, multiply with RunSpeedModifier
        if (crouchFlag) { xVal *= crouchSpeedModifier; }
        // Create Vec2 for velocity
        Vector2 targetVelocity = new Vector2(xVal, rb.linearVelocity.y);
        // Set player velocity
        if (!isHit)
        {
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector2(knockbackVelocity.x, rb.linearVelocity.y);
        }

        // Look right right, turning left
        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        // Looking left, turning right
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        #endregion

    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void StartInvincibility()
    {
        StartCoroutine(InvincibilityFrames());
    }
    public void Die()
    {
        isDead = true;

        StopAllCoroutines();
        // Reset states + velocity
        isHit = false;
        isInvincible = false;
        knockbackVelocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        // Make sure sprite is normal upon death
        spriteRenderer.enabled = true;

        if (extraLives > 0)
        {
            extraLives--;

            ResetPlayer();
            StartInvincibility();
        }
        else
        {
            // Reload entire scene (clean reset)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void ResetPlayer()
    {
        // Stop absolutely everything
        StopAllCoroutines();

        // Reset states
        isDead = false;
        isHit = false;
        isInvincible = false;

        // Reset movement
        horizontalValue = 0;
        knockbackVelocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        // Make sure sprite is visible
        spriteRenderer.enabled = true;

        ResetHealth();
    }

    public void ResetHealth()
    {
        HealthBar hp = FindFirstObjectByType<HealthBar>();
        hp.Heal(100);
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isInvincible)
            return;

        HealthBar hp = FindFirstObjectByType<HealthBar>();
        hp.LoseHealth(damage);

        if (hp.GetCurrentHealth() <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(HitRoutine(attackerPosition));
    }

    IEnumerator HitRoutine(Vector2 attackerPosition)
    {
        // Enable temporary invincibility so we don't take damage again
        isInvincible = true;

        // Mark the player as being hit (disables movement control)
        isHit = true;

        // Determine knockback direction
        // If the attacker is on the right ? push player left
        // If the attacker is on the left ? push player right
        float direction = transform.position.x < attackerPosition.x ? -1f : 1f;

        // Create the knockback velocity (horizontal + upward force)
        knockbackVelocity = new Vector2(direction * knockbackForce, knockbackUpForce);

        // Immediately apply knockback velocity
        rb.linearVelocity = knockbackVelocity;

        float timer = 0f;

        // Flicker effect loop during invincibility duration
        while (timer < invincibleDuration)
        {
            // Toggle sprite visibility to create flicker effect
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flickerInterval);

            timer += flickerInterval;
        }

        // Ensure sprite is visible at the end
        spriteRenderer.enabled = true;

        // Reset knockback
        knockbackVelocity = Vector2.zero;

        // Freeze movement only for hit stun duration
        yield return new WaitForSeconds(hitStunDuration);

        // Allow movement again
        isHit = false;

        // Continue immunity until full invincibility duration finishes
        yield return new WaitForSeconds(invincibleDuration - hitStunDuration);

        isInvincible = false;
    }
}

   
