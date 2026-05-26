using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    public float moveSpeed = 8f;
    public float jumpForce = 24f;
    public float dashSpeed = 16f;
    public float dashDuration = 0.4f;

    [Range(0f, 1f)]
    public float jumpCutMultiplier = 0.6f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Dash Trail & Jump")]
    public GameObject ghostPrefab;
    public float ghostSpawnRate = 0.05f;
    private float ghostTimer;

    [Tooltip("Dash force multiplier applied to the jump when the player jumps during a dash.")]
    public float dashJumpForceMultiplier = 1.15f;

    [Header("Controls System")]
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction dashAction;

    [Header("Visual Effects")]
    public GameObject dashDustPrefab;
    public Transform dustSpawnPoint;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool isGrounded;
    private bool isDashing;
    private float dashTimeLeft;
    private bool isDashJumping;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        dashAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        horizontalInput = moveAction.ReadValue<float>();

        if (!isGrounded && rb.linearVelocity.y <= 0)
        {
            isDashJumping = false;
        }

        // Logica de salto
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            float currentJumpForce = jumpForce;

            if (isDashing)
            {
                isDashing = false;
                isDashJumping = true;
                currentJumpForce = jumpForce * dashJumpForceMultiplier;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpForce);
            isGrounded = false;
        }

        if (jumpAction.WasPressedThisFrame() && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        if (dashAction.WasPressedThisFrame() && !isDashing && isGrounded)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;

            TriggerDashEffect();
        }

        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;

        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", isDashing);

        if (isDashing || isDashJumping)
        {
            ghostTimer -= Time.deltaTime;

            if (ghostTimer <= 0)
            {
                SpawnGhost();
                ghostTimer = ghostSpawnRate;
            }
        } else
        {
            ghostTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                float dashDirection = spriteRenderer.flipX ? -1f : 1f;
                rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
                dashTimeLeft -= Time.fixedDeltaTime;
            } else
            {
                isDashing = false;
            }
        } else if (isDashJumping)
        {
            rb.linearVelocity = new Vector2(horizontalInput * dashSpeed, rb.linearVelocity.y);   
        } 
        else
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void TriggerDashEffect()
    {
        if (dashDustPrefab == null || dustSpawnPoint == null) return;

        Quaternion dustRotation = spriteRenderer.flipX ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        Instantiate(dashDustPrefab, dustSpawnPoint.position, dustRotation);
    }

    void SpawnGhost()
    {
        if (ghostPrefab == null) return;

        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();

        ghostSR.sprite = spriteRenderer.sprite;
        ghostSR.flipX = spriteRenderer.flipX;
    }
}
