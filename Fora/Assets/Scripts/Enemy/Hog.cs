using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hog : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private DetectionZone _attackZone;

    public Vector2 _directionVector;

    private Animator _animator;
    private Rigidbody2D _rb;
    private WalkDirection _direction;  
    private EnemyConditions _enemyConditions;
    private float _walkStopRate = 0.05f;

    public WalkDirection Direction
    {
        get { return _direction; }
        set {

            if (_direction != value)
            {
                //Direction flipped
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

                if (value == WalkDirection.Right)
                {
                    _directionVector = Vector2.right;
                }
                else if (value == WalkDirection.Left)
                {
                    _directionVector = Vector2.left;
                }

            }
            _direction = value; }
    }

    private bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            _animator.SetBool("hasTarget", value);
        }
    }

    public bool CanMove
    {
        get
        {
            return _animator.GetBool("canMove");
        }
    }

    public enum WalkDirection { Right, Left }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyConditions = GetComponent<EnemyConditions>();
        _animator = GetComponent<Animator>();
        _directionVector = Vector2.right;
    }

    private void FixedUpdate()
    {
        if (_enemyConditions.IsCollidingWall)
        {
            FlipDirection();
        }

        if (CanMove)
        {
            _rb.velocity = new Vector2(_walkSpeed * _directionVector.x, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, _walkStopRate), _rb.velocity.y);
        }
    }

    private void Update()
    {
        HasTarget = _attackZone.DetectedColliders.Count > 0;
    }

    private void FlipDirection()
    {
        if (Direction == WalkDirection.Right)
        {
            Direction = WalkDirection.Left;
        }
        else if (Direction == WalkDirection.Left)
        {
            Direction = WalkDirection.Right;
        }
    }

}
