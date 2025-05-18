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

    private Rigidbody2D rb;
    private bool isGrounded;
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

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump triggered!");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Ground check using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("Grounded: " + isGrounded);

        // Move
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    // Public accessors
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }

    // ✅ Visualize the ground check area in Scene View
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
