using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float _fallMultiplier = 2f;

    [Header("Collisions")]
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int verticalRayCount = 4;
    [SerializeField] private int horizontalRayCount = 4;

    #region Properties
    // Return if the Player is facing right
    public bool FacingRight { get; set; }

    // Return the Gravity value
    public float Gravity => gravity;

    // Return the Force applied
    public Vector2 Force => _force;

    // Return the conditions
    public PlayerConditions Conditions => _conditions;
    #endregion

    #region Internal

    private CircleCollider2D _circleCollider2D;
    private PlayerConditions _conditions;

    private Vector2 _boundsTopLeft;
    private Vector2 _boundsTopRight;
    private Vector2 _boundsBottomLeft;
    private Vector2 _boundsBottomRight;

    private float _boundsWidth; 
    private float _boundsHeight;

    private float _currentGravity;
    private Vector2 _force;
    private Vector2 _movePosition;
    private float _skin = 0.01f;

    private float _internalFaceDirection = 1f; // 1 for right, -1 for left
    private float _faceDirection;

    #endregion

    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();   

        _conditions = new PlayerConditions();
        _conditions.Reset();
    }

    private void Update()
    {
        ApplyGravity();
        StartMovement();

        SetRayOrigins();
        GetFaceDirection();

        if (FacingRight)
        {
            HorizontalCollision(1);
        }
        else
        {
            HorizontalCollision(-1);
        }

        CollisionBelow();
        CollisionAbove();

        transform.Translate(_movePosition, Space.Self);

        SetRayOrigins();
        CalculateMovement();

    }

    #region Ray Origins

    // Calculate ray based on our collider

    private void SetRayOrigins()
    {
        Bounds playerBounds = _circleCollider2D.bounds;

        _boundsBottomLeft = new Vector2(playerBounds.min.x, playerBounds.min.y);
        _boundsBottomRight = new Vector2(playerBounds.max.x, playerBounds.min.y);
        _boundsTopLeft = new Vector2(playerBounds.min.x, playerBounds.max.y);
        _boundsTopRight = new Vector2(playerBounds.max.x, playerBounds.max.y);

        _boundsHeight = Vector2.Distance(_boundsBottomLeft, _boundsTopLeft);
        _boundsWidth = Vector2.Distance(_boundsBottomLeft, _boundsBottomRight);

    }
    #endregion

    #region Movement

    // Clamp our fource applied
    private void CalculateMovement()
    {
        if (Time.deltaTime > 0)
        {
            _force = _movePosition / Time.deltaTime;
        }
    }

    // Initialize the movePosition
    private void StartMovement()
    {
        _movePosition = _force * Time.deltaTime;
        _conditions.Reset();
    }

    // Set new x movement
    public void SetHorizontalForce(float xForce)
    {
        _force.x = xForce;
    }

    public void SetVerticalForce(float yForce)
    {
        _force.y = yForce;
    }

    // Calculate the gravity to apply
    private void ApplyGravity()
    {
        _currentGravity = gravity;

        if (_force.y < 0)
        {
            _currentGravity *= _fallMultiplier; // Apply fall multiplier if falling
        }

        _force.y += _currentGravity * Time.deltaTime;
    }

    #endregion

    #region Direction
    // Manage the direction player facing
    private void GetFaceDirection()
    {
        _faceDirection = _internalFaceDirection;
        FacingRight = _faceDirection == 1; // if FacingRight is true

        if (_force.x > 0.0001f)
        {
            _faceDirection = 1f;
            FacingRight = true;
        }
        else if (_force.x < -0.0001f)
        {
            _faceDirection = -1f;
            FacingRight = false;
        }
        else
        {
             _internalFaceDirection = _faceDirection;
        }
        
    }
    #endregion

    #region Collisions
    #region Collision Below

    private void CollisionBelow()
    {
        if (_movePosition.y < 0.0001f)
        {
            _conditions.IsFalling = true;
        }
        else
        {
            _conditions.IsFalling = false;
        }

        if (!_conditions.IsFalling)
        {
            _conditions.IsCollidingBelow = false;
            return;
        }

        // Calculate ray length
        float rayLength = _boundsHeight / 2f + _skin;
        if (_movePosition.y < 0)
        {
            rayLength += Mathf.Abs(_movePosition.y);
        }

        // Calculate ray origin
        Vector2 leftOrigin = (_boundsBottomLeft + _boundsTopLeft) / 2f;
        Vector2 rightOrigin = (_boundsBottomRight + _boundsTopRight) / 2f;
        leftOrigin += (Vector2)(transform.up * _skin) + (Vector2)(transform.right * _movePosition.x);
        rightOrigin += (Vector2)(transform.up * _skin) + (Vector2)(transform.right * _movePosition.x);

        // Raycast
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(leftOrigin, rightOrigin, (float)i / (float)(verticalRayCount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -transform.up, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, -transform.up * rayLength, Color.green);

            if (hit)
            {
                if (_force.y > 0)
                {
                    _movePosition.y = _force.y * Time.deltaTime;
                    _conditions.IsCollidingBelow = false;
                }
                else
                {
                    _movePosition.y = -hit.distance + _boundsHeight / 2f + _skin;
                }
                    

                _conditions.IsCollidingBelow = true;
                _conditions.IsFalling = false;

                if (Mathf.Abs(_movePosition.y) < 0.00001f)
                {
                    _movePosition.y = 0f;
                }
            }
            
        }
    }

    #endregion
    #region Collision Horizontal

    private void HorizontalCollision(int direction)
    {
        Vector2 rayHorizontalBottom = (_boundsBottomLeft + _boundsBottomRight) / 2f;
        Vector2 rayHorizontalTop = (_boundsTopLeft + _boundsTopRight) / 2f;
        rayHorizontalBottom += (Vector2)(transform.up * _skin);
        rayHorizontalTop -= (Vector2)(transform.up * _skin);

        float rayLength = Mathf.Abs(_force.x * Time.deltaTime) + _boundsWidth / 2f + _skin;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayHorizontalBottom, rayHorizontalTop, (float)i / (horizontalRayCount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * transform.right, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, transform.right * rayLength * direction, Color.cyan);

            if (hit)
            {
                if (direction >= 0)
                {
                    _movePosition.x = hit.distance - _boundsWidth / 2f - _skin * 2f;
                    _conditions.IsCollidingRight = true;
                }
                else
                {
                    _movePosition.x = -hit.distance + _boundsWidth / 2f + _skin * 2f;
                    _conditions.IsCollidingLeft = true;
                }

                _force.x = 0f;
            }
        }

    }
    #endregion
    #region Collision Above

    private void CollisionAbove()
    {
        if (_movePosition.y < 0)
        {
            return; // No need to check if moving down
        }

        // Set ray length
        float rayLength = _movePosition.y + _boundsHeight / 2f;

        // Origin Points
        Vector2 rayTopLeft = (_boundsBottomLeft + _boundsTopLeft) / 2f;
        Vector2 rayTopRight = (_boundsBottomRight + _boundsTopRight) / 2f;
        rayTopLeft += (Vector2)transform.right * _movePosition.x;
        rayTopRight += (Vector2)transform.right * _movePosition.x;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayTopLeft, rayTopRight, (float)i / (float)(verticalRayCount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.red);

            if (hit)
            {
                _movePosition.y = hit.distance - _boundsHeight / 2f;
                _conditions.IsCollidingAbove = true;
            }
        }

    }
    #endregion
    #endregion
}
