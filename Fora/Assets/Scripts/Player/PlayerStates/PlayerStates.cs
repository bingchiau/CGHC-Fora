using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    protected PlayerController _playerController;
    protected float _horizontalInput;
    protected float _verticalInput;

    protected virtual void Start()
    {
        InitState();
    }

    // We call some logic need in the start
    protected virtual void InitState()
    {
        _playerController = GetComponent<PlayerController>();       
    }

    // Override to create state logic
    public virtual void ExecuteState()
    {
        // Default implementation does nothing
    }

    // Gets the input from the player
    public virtual void LocalInput()
    {
        if (_playerController.Conditions.IsBouncing)
        {
            _horizontalInput = 0f;
            return;
        }
        else if (_playerController.Conditions.IsCollidingLeft)
        {
            if (_horizontalInput < 0f)
            {
                _horizontalInput = 0f;
                return;
            }
            
        }
        else if (_playerController.Conditions.IsCollidingRight)
        {
            if (_horizontalInput > 0f)
            {
                _horizontalInput = 0f;
                return;
            }
        }

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
       
        GetInput();
    }

    // Override to get specifc input
    protected virtual void GetInput()
    {
        // Default implementation does nothing
    }
}
