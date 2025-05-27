using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float gravity = 20f;
    public float terminalVelocity = -25f;

    public float radius = 0.5f;
    public LayerMask obstacleLayer;

    public float groundRayLength = 0.1f;

    private float verticalVelocity = 0f;
    private float horizontalInput = 0f;
    private bool jumpInput = false;

    [Range(0f, 1f)]
    public float bounceFactor = 0.6f; // 1 = perfect bounce, 0 = no bounce
    public float minBounceVelocity = 1f; // below this = no more bounce

    public int maxBounces = 3;     // how many times to bounce
    private int bounceCount = 0;   // current bounce count

    private bool isGrounded;

    void Update()
    {
        // Capture input here
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        // Jump logic
        if (isGrounded && jumpInput)
        {
            verticalVelocity = jumpForce;
            jumpInput = false;
            bounceCount = 0; // Reset when jumping manually
        }
    }

    void FixedUpdate()
    {
        isGrounded = IsGrounded();

        // Gravity simulation
        bool shouldZeroVelocity = isGrounded && Mathf.Abs(verticalVelocity) < minBounceVelocity;

        if (!shouldZeroVelocity)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            verticalVelocity = 0;
        }

        verticalVelocity = Mathf.Max(verticalVelocity, terminalVelocity);

        Vector2 move = Vector2.zero;

        // Horizontal movement
        if (horizontalInput != 0 && !IsHittingWall(new Vector2(horizontalInput, 0)))
        {
            move.x = horizontalInput * moveSpeed * Time.fixedDeltaTime;
        }

        // Vertical movement with raycast collision
        if (verticalVelocity < 0) // Falling
        {
            float distToGround = radius + groundRayLength;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distToGround, obstacleLayer);
            Debug.DrawRay(transform.position, Vector2.down * distToGround, Color.cyan);

            if (hit.collider != null)
            {
                float gap = hit.distance - radius;

                if (verticalVelocity < -minBounceVelocity && bounceCount < maxBounces)
                {
                    verticalVelocity = -verticalVelocity * bounceFactor;
                    bounceCount++;
                }
                else
                {
                    verticalVelocity = 0f;
                }

                // Prevent snapping below surface
                float bounceMove = verticalVelocity * Time.fixedDeltaTime;
                if (bounceMove < -gap)
                {
                    move.y = -gap;
                }
                else
                {
                    move.y = bounceMove;
                }

            }
            else
            {
                move.y = verticalVelocity * Time.fixedDeltaTime;
            }
        }
        else
        {
            move.y = verticalVelocity * Time.fixedDeltaTime;
        }

        transform.Translate(move);
    }

    bool IsHittingWall(Vector2 dir)
    {
        Vector2 origin = transform.position;
        float[] yOffsets = new float[] { radius * 0.8f, 0f, -radius * 0.8f };

        foreach (float y in yOffsets)
        {
            Vector2 start = origin + new Vector2(0, y);
            float horizontalLength = Mathf.Sqrt(Mathf.Max(radius * radius - y * y, 0.0001f));
            RaycastHit2D hit = Physics2D.Raycast(start, dir.normalized, horizontalLength + 0.02f, obstacleLayer);
            Debug.DrawRay(start, dir.normalized * (horizontalLength + 0.02f), Color.red);
            if (hit.collider != null)
                return true;
        }

        return false;
    }

    bool IsGrounded()
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, radius + groundRayLength, obstacleLayer);
        Debug.DrawRay(origin, Vector2.down * (radius + groundRayLength), Color.green);
        return hit.collider != null;
    }

    // ✅ ADD THESE BELOW:
    public bool IsGroundedPublic()
    {
        return isGrounded;
    }

    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }
}