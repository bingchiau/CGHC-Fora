using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;

    [Header("Bounce Settings")]
    public float bounceForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool hasBounceCharge = false;

    private float horizontalInput;

    public static CharacterMovement Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasBounceCharge = true; // ✅ gain 1 bounce after jump
            Debug.Log("Jump triggered, bounce charge granted");
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ✅ On landing
        if (!wasGroundedLastFrame && isGrounded)
        {
            if (hasBounceCharge)
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);
                hasBounceCharge = false; // ✅ use up the bounce
                Debug.Log("Bounce triggered!");
            }
        }

        // Horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        wasGroundedLastFrame = isGrounded;
    }

    public bool IsGrounded() => isGrounded;
    public float GetVerticalVelocity() => rb.velocity.y;

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
