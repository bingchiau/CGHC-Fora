using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _jumpHeight = 4f;
    [SerializeField] private int _maxJumps = 2;

    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        float jumpForce = Mathf.Sqrt(_jumpHeight * 2f * Mathf.Abs(_playerController.Gravity));
        _playerController.SetVerticalForce(jumpForce);
    }
}
