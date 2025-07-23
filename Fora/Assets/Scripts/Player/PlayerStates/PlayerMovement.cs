using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _minSpeed = 3f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private float speedThreshold = 0.1f;

    private float _horizontalMovement;
    private float _movement;

    protected override void InitState()
    {
        base.InitState();
    }

    public override void ExecuteState()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {   
        if (Mathf.Abs(_horizontalMovement) > 0.1f)
        {
            _movement = _horizontalMovement;
        }
        else
        {
            _movement = 0f;
        }

        float moveSpeed = EvaluateWeight(_maxSpeed, _minSpeed);

        if (_playerController.Conditions.IsBouncing)
        {
            moveSpeed = _movement * 3f;
        }
        else
        {
            moveSpeed = _movement * moveSpeed;
        }

        if (!CanMove)
        {
            moveSpeed = 0f;
            _playerController.SetVerticalForce(0f);
        }

        moveSpeed = EvaluateFriction(moveSpeed);
        _playerController.SetHorizontalForce(moveSpeed);

        if (animator != null)
        {
            float animationSpeed = Mathf.Abs(moveSpeed);
            animator.SetFloat(speedParam, animationSpeed);
        }
    }

    protected override void GetInput()
    {
        _horizontalMovement = _horizontalInput;
    }

    private float EvaluateFriction(float moveSpeed)
    {
        if (_playerController.Friction > 0)
        {
            moveSpeed = Mathf.Lerp(_playerController.Force.x, moveSpeed, _playerController.Friction * 10f * Time.deltaTime);
        }
        return moveSpeed;
    }

    private float EvaluateWeight(float max, float min)
    {
        return Mathf.Lerp(max, min, _playerController.WeightRatio);
    }
}
