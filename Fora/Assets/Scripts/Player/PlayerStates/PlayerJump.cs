using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _maxJumpHeight = 5f;
    [SerializeField] private float _minJumpHeight = 1f;
    [SerializeField] private int _maxJumps = 1;

    // Return how many jumps player has left
    public int JumpsLeft { get; private set; }

    protected override void InitState()
    {
        base.InitState();
        JumpsLeft = _maxJumps;
    }

    public override void ExecuteState()
    {
        if (_playerController.Conditions.IsCollidingBelow && _playerController.Force.y == 0f)
        {
            JumpsLeft = _maxJumps; // Reset jumps when colliding below
            _playerController.Conditions.IsJumping = false; // Reset jumping condition
        }

    }

    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (!CanJump())
        {
            return;
        }
        if (JumpsLeft == 0)
        {
            return;
        }
        
        JumpsLeft--;

        float jumpForce = EvaluateWeight(_maxJumpHeight, _minJumpHeight); // Adjust jump force based on player weight
        jumpForce = Mathf.Sqrt(jumpForce * 2f * Mathf.Abs(_playerController.Gravity));
        
        Debug.Log("jumpForce" + jumpForce);
        _playerController.SetVerticalForce(jumpForce);
        _playerController.Conditions.IsJumping = true;
    }

    private bool CanJump()
    {
        if (!_playerController.Conditions.IsCollidingBelow && JumpsLeft <= 0)
        {
            return false; // Cannot jump if not colliding below and no jumps left
        }

        return true; // Can jump if colliding below or has jumps left
    }

    private float EvaluateWeight(float max, float min)
    {
        float jumpForce = Mathf.Lerp(max, min, _playerController.WeightRatio);
        return jumpForce;
    }
}
