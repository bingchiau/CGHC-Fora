using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    // ✅ Public method for camera to check if grounded
    public bool IsGrounded()
    {
        return isGrounded;
    }

    // ✅ Public method for camera to get vertical velocity
    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }
}
