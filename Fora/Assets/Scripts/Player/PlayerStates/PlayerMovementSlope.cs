using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovementSlope : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 6f;
    public float gravity = 20f;
    public LayerMask groundLayer;
    public LayerMask slopeLayer;

    private Vector2 velocity;
    private CircleCollider2D col;
    private Vector2 input;
    private bool grounded;
    private bool jumpPressed;

    void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        // Capture jump input here — only true for one frame
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        grounded = IsGrounded();
        bool tryingToClimb = Input.GetKey(KeyCode.W);

        if (grounded)
        {
            if (jumpPressed)
            {
                velocity.y = jumpForce;
            }
            else
            {
                MoveAlongSlope(input);
            }
        }
        else
        {
            // In-air movement
            velocity.x = input.x * moveSpeed;
            velocity.y -= gravity * Time.fixedDeltaTime;
        }

        ApplyGroundSnap();

        transform.position += (Vector3)(velocity * Time.fixedDeltaTime);

        // Reset the jump flag AFTER processing
        jumpPressed = false;
    }


    /*void FixedUpdate()
    {
        grounded = IsGrounded();
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        bool tryingToClimb = Input.GetKey(KeyCode.W);
        bool tryingToJump = Input.GetKeyDown(KeyCode.Space);

        // Horizontal movement allowed always (except climbing slope requires grounded)
        if (input.x != 0)
        {
            if (grounded && tryingToClimb)
            {
                AttemptSlopeClimb();
            }
            else
            {
                MoveHorizontal();
            }
        }
        else
        {
            velocity.x = 0;
        }

        if (grounded && tryingToJump)
        {
            velocity.y = jumpForce;
        }

        ApplyGravity();

        transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
    }*/

    void OnDrawGizmos()
    {
        if (col == null) return;

        float radius = col.radius * transform.localScale.x;
        Vector2 center = (Vector2)transform.position + col.offset;

        int rayCount = 16;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = (360f / rayCount) * i * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 origin = center + dir * radius;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, origin + dir * 0.1f);
        }
    }

    void MoveHorizontal()
    {
        Vector2 origin = (Vector2)transform.position + col.offset;
        Vector2 dir = new Vector2(Mathf.Sign(input.x), 0);

        // Check collision only if grounded; else no blocking horizontally in air
        bool blocked = false;
        if (grounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, col.radius + 0.05f, groundLayer);
            Debug.DrawLine(origin, origin + dir * (col.radius + 0.05f), Color.green);
            blocked = hit.collider != null;
        }

        if (!blocked)
            velocity.x = input.x * moveSpeed;
        else
            velocity.x = 0;
    }

    void AttemptSlopeClimb()
    {
        Vector2 origin = (Vector2)transform.position + col.offset;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, col.radius + 0.2f, slopeLayer);
        Debug.DrawLine(origin, origin + Vector2.down * (col.radius + 0.2f), Color.red);

        if (hit)
        {
            Vector2 slopeNormal = hit.normal;
            Vector2 slopeDir = new Vector2(slopeNormal.y, -slopeNormal.x);

            if (Mathf.Sign(slopeDir.x) != Mathf.Sign(input.x))
                slopeDir = -slopeDir;

            velocity.x = slopeDir.x * moveSpeed * Mathf.Abs(input.x);
            velocity.y = slopeDir.y * moveSpeed * Mathf.Abs(input.x);
        }
        else
        {
            velocity.x = input.x * moveSpeed;
        }
    }

    void ApplyGravity()
    {
        if (!grounded)
            velocity.y -= gravity * Time.fixedDeltaTime;
        else if (velocity.y < 0)
            velocity.y = 0;
    }

    bool IsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + col.offset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, col.radius + 0.05f, groundLayer | slopeLayer);
        Debug.DrawLine(origin, origin + Vector2.down * (col.radius + 0.05f), Color.blue);
        return hit.collider != null;
    }

    void MoveAlongSlope(Vector2 input)
    {
        // Get the ground normal
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, col.radius + 0.1f, groundLayer | slopeLayer);
        if (hit)
        {
            Vector2 slopeNormal = hit.normal;
            Vector2 slopeTangent = new Vector2(slopeNormal.y, -slopeNormal.x);
            Vector2 moveDir = slopeTangent.normalized * input.x * moveSpeed;
            velocity = moveDir;
        }
        else
        {
            // Fallback to horizontal movement
            velocity = new Vector2(input.x * moveSpeed, velocity.y);
        }
    }

    void ApplyGroundSnap()
    {
        if (!grounded)
        {
            // Apply a small downward force to keep the character grounded
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
    }

}
