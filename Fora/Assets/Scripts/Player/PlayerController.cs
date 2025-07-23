using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _fallMultiplier = 2f;
    [SerializeField] private float _weight = 1f;

    [Header("Collisions")]
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int verticalRayCount = 7;
    [SerializeField] private int horizontalRayCount = 7;

    [Header("Movement")]
    [Tooltip("Maximum slope (in degrees) the player can walk up")]
    [SerializeField] private float maxSlopeAngle = 45f;

    #region Properties
    // Return if the Player is facing right
    public bool FacingRight { get; set; }

    // Return the Gravity value
    public float Gravity => _gravity;

    // Return the Force applied
    public Vector2 Force => _force;

    // Return the conditions
    public PlayerConditions Conditions => _conditions;

    // Set and return the friction value
    public float Friction { get; set; }
    
    // Return the weight value
    public float WeightRatio => _weightRatio;
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

    private Vector2 _hitPoint;
    private Vector2 bounceDir;

    private float _maxWeight = 5f;
    private float _minWeight = 1f;
    private float _weightRatio;

    private float _currentGravity;
    private float _originalGravity;
    private Vector2 _force;
    private Vector2 _movePosition;
    //private float _skin = 0.06f;
    // Alan change
    private float _skin = 0.02f;

    private bool _checkStop = false;

    private float _internalFaceDirection = 1f; // 1 for right, -1 for left
    private float _faceDirection;

    private DeadZone _deadZone;

    #endregion

    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();   

        _conditions = new PlayerConditions();
        _conditions.Reset();

        _weightRatio = _weight / _maxWeight;
        UIManager.Instance.UpdateWeight(_weightRatio); // Update weight at the start

        _originalGravity = _gravity;

        // Alan change (- prevent collision with trigger collider)
        Physics2D.queriesHitTriggers = false;
    }

    private void Update()
    {
        ApplyGravity();
        StartMovement();

        SetRayOrigins();
        GetFaceDirection();
        RotateModel();

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

        if (_weight <= _minWeight)
        {
            _weight = _minWeight; // Ensure weight is never zero or negative
        }
        else if (_weight > _maxWeight)
        {
            _weight = _maxWeight; // Clamp weight to a maximum value
        }
        _weightRatio = _weight / _maxWeight; // Calculate weight ratio

        //transform.Translate(_movePosition, Space.Self);
        // Alan change
        transform.position += (Vector3)_movePosition;
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

    public void SetForce(Vector2 force)
    {
        _force = force;
    }

    // Calculate the gravity to apply
    private void ApplyGravity()
    {
        _currentGravity = _gravity;

        if (_force.y < 0)
        {
            _currentGravity *= _fallMultiplier; // Apply fall multiplier if falling
        }

        _force.y += _currentGravity * Time.deltaTime;
    }

    public void StopGravity()
    {
        _originalGravity = _gravity;
        _gravity = 0f;
    }

    public void ResumeGravity()
    {
        _gravity = _originalGravity;
    }

    #endregion

    #region Weight

    public void AddWeight(float weight)
    {
        _weight += weight * Time.deltaTime;
        if (_weight > _maxWeight)
        {
            _weight = _maxWeight;
        }

        _weightRatio = _weight / _maxWeight;

        UIManager.Instance.UpdateWeight(_weightRatio); // Update UI
    }

    public void AddWeight_oneTime(float weight)
    {
        _weight += weight;
        if (_weight > _maxWeight)
        {
            _weight = _maxWeight;
        }

        _weightRatio = _weight / _maxWeight;

        UIManager.Instance.UpdateWeight(_weightRatio); // Update UI
    }

    public void ReduceWeight(float weight)
    {
        _weight -= weight;
        if (_weight < _minWeight)
        {
            _weight = _minWeight;
        }

        _weightRatio = _weight / _maxWeight; // Recalculate weight ratio

        UIManager.Instance.UpdateWeight(_weightRatio); // Update UI 
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

        _internalFaceDirection = _faceDirection;

        
    }

    private void RotateModel()
    {
        if (FacingRight)
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); // Face right
        }
        else
        {
            transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f); // Face left
        }
    }
    #endregion

    #region Collisions
    #region Collision Below

    private void CollisionBelow()
    {
        Friction = 0f;

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
        float rayLength;
        float turnAngle = 22.5f;
        

        // Calculate ray origin
        Vector2 leftOrigin = (_boundsBottomLeft + _boundsTopLeft) / 2f;
        Vector2 rightOrigin = (_boundsBottomRight + _boundsTopRight) / 2f;
        leftOrigin += (Vector2)(transform.up * _skin) + (Vector2)(transform.right * _movePosition.x);
        rightOrigin += (Vector2)(transform.up * _skin) + (Vector2)(transform.right * _movePosition.x);

        // Raycast
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(leftOrigin, rightOrigin, (float)i / (float)(verticalRayCount - 1));

            //rayLength = Mathf.Round((_boundsHeight / 2f + _skin) * Mathf.Sin(turnAngle * Mathf.Deg2Rad) * 1000.0f) * 0.001f; // Adjust ray length based on angle
            // Alan change
            rayLength = (_boundsHeight / 2f + _skin) * Mathf.Max(Mathf.Sin(turnAngle * Mathf.Deg2Rad), 0.1f);
            float temp = rayLength;
            if (_movePosition.y < 0)
            {
                
                rayLength += Mathf.Abs(_movePosition.y);
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -transform.up, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, -transform.up * rayLength, Color.green);
            turnAngle += 22.5f;

            if (hit)
            {
                GameObject hitObject = hit.collider.gameObject;
                
                if (Conditions.IsDashing)
                {
                    _hitPoint = hit.normal;
                }
                else
                {
                    _hitPoint = Vector2.zero;
                }
                //Debug.DrawRay(hit.point, hit.normal, Color.green);

                if (_force.y > 0)
                {
                    _movePosition.y = _force.y * Time.deltaTime;
                    _conditions.IsCollidingBelow = false;
                }
                else
                {
                    _force.y = 0f; // Reset vertical force when colliding below
                    _movePosition.y = -hit.distance + temp;
                }

                _conditions.IsCollidingBelow = true;
                _conditions.IsFalling = false;

                if (Mathf.Abs(_movePosition.y) < 0.00001f)
                {
                    _movePosition.y = 0f;
                }

                if (hitObject.GetComponent<SpecialSurface>() != null)
                {
                    Friction = hitObject.GetComponent<SpecialSurface>().Friction;
                }

                if (hitObject.CompareTag("DeadZone"))
                {
                    _deadZone = hitObject.GetComponent<DeadZone>();

                    if (_deadZone != null)
                    {
                        _deadZone.Die(this.gameObject);
                    }
                }

                //_conditions.IsBouncing = false;

            }

        }

    }

    #endregion
    #region Collision Horizontal

    private void HorizontalCollision(int direction)
    {
        Vector2 rayBottom = (_boundsBottomLeft + _boundsBottomRight) * 0.5f;
        Vector2 rayTop = (_boundsTopLeft + _boundsTopRight) * 0.5f;
        rayBottom += (Vector2)transform.up * _skin;
        rayTop -= (Vector2)transform.up * _skin;

        float rayLength = Mathf.Abs(_force.x * Time.deltaTime) + _boundsWidth / 2f + _skin;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayBottom, rayTop, (float)i / (horizontalRayCount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * transform.right, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, transform.right * rayLength * direction, Color.cyan);


            if (!hit) continue;

            _hitPoint = hit.normal;
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            //_conditions.IsBouncing = false;

            if (!_checkStop)
            {
                _force.x = 0f; // Reset horizontal force when colliding
                _force.y = 0f; 
                _checkStop = true; // Stop further raycasts after the first hit
            }

            // 1) Check slope angle
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle > 0 && slopeAngle <= maxSlopeAngle)
            {
                float moveDistance = Mathf.Abs(_force.x * Time.deltaTime);

                // 2) Walk up slope
                // Rotate the surface normal by -90 degrees then multiply with direction to get (tangent) vector up the slope.
                Vector2 slopeTangent = new Vector2(hit.normal.y, -hit.normal.x) * direction;
                slopeTangent.Normalize();
                Debug.DrawRay(hit.point, slopeTangent, Color.red);
                Debug.DrawRay(hit.point, hit.normal, Color.green);

                // 3) Recompute movement along slope
                _movePosition = slopeTangent * moveDistance;
                
                // 4) Zero out gravity-induced fall and mark grounded
                if (_force.y > 0 && _conditions.IsJumping == true)
                {
                    _checkStop = false;
                    _movePosition.y = _force.y * Time.deltaTime; // allow jump
                    _conditions.IsCollidingBelow = false;
                    _conditions.IsOnSlope = false;

                    // Alan - For platform follow (Disable follow when jumping)
                    transform.SetParent(null);

                }

                _force.y = 0f;
                _conditions.IsOnSlope = true;
                _conditions.SlopeAngle = slopeAngle;
                _conditions.IsCollidingBelow = true;

                return; // we handled movement, skip “wall” logic
            }

            

            // 5) Otherwise it’s a wall – block horizontal movement
            if (direction > 0)
            {
                _movePosition.x = hit.distance - _boundsWidth / 2f - _skin;
                _conditions.IsCollidingRight = true;
            }
            else
            {
                _movePosition.x = -hit.distance + _boundsWidth / 2f + _skin;
                _conditions.IsCollidingLeft = true;
            }

            _force.x = 0f;
            return;
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
        float rayLength;
        float turnAngle = 22.5f;

        // Origin Points
        Vector2 rayTopLeft = (_boundsBottomLeft + _boundsTopLeft) / 2f;
        Vector2 rayTopRight = (_boundsBottomRight + _boundsTopRight) / 2f;
        rayTopLeft += (Vector2)transform.right * _movePosition.x;
        rayTopRight += (Vector2)transform.right * _movePosition.x;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayTopLeft, rayTopRight, (float)i / (float)(verticalRayCount - 1));

            rayLength = Mathf.Round((_boundsHeight / 2f + _skin) * Mathf.Sin(turnAngle * Mathf.Deg2Rad) * 1000.0f) * 0.001f; // Adjust ray length based on angle
            float temp = rayLength;

            if (_movePosition.y > 0)
            {
                rayLength += Mathf.Abs(_movePosition.y);
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.red);
            turnAngle += 22.5f;

            if (hit)
            {
                if (Conditions.IsDashing)
                {
                    _hitPoint = hit.normal;
                }
                else
                {
                    _hitPoint = Vector2.zero;
                }
                _movePosition.y = hit.distance - temp;
                _conditions.IsCollidingAbove = true;
                //_conditions.IsBouncing = false;
            }
        }

    }
    #endregion

    public Vector2 Bounce(Vector2 inDirection)
    {
        //Debug.Log("yes bounce");
        bounceDir =  Vector2.Reflect(inDirection, _hitPoint);
        return bounceDir;
    }
    #endregion

    // Alan - For platform follow transform
    public void ApplyPlatformOffset(Vector3 offset)
    {
        transform.position += offset;
    }
}
