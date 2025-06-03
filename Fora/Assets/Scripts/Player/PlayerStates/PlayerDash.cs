using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerDash : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _dashPower = 17f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;
    [SerializeField] private int _maxDashes = 1;
    [SerializeField] private TrailRenderer _dashTrail;
    
    private bool _finishCooldown = false;

    public int DashLeft { get; private set; }
    protected override void InitState()
    {
        base.InitState();
        DashLeft = _maxDashes;
        _dashTrail = GetComponent<TrailRenderer>();
    }

    public override void ExecuteState()
    {
        if (_playerController.Conditions.IsDashing)
        {
            return;
        }

        if (_playerController.Conditions.IsCollidingBelow && _finishCooldown)
        {
            DashLeft = _maxDashes;
        }
    }

    protected override void GetInput()
    {
        if (Input.GetKey(KeyCode.J))
        {
            Dash();
        }
    }

    private void Dash()
    {
        if (!CanDash())
        {
            return;
        }

        StartCoroutine(DashCountdown());
    }

    private bool CanDash()
    {
        if (_playerController.Conditions.IsCollidingRight || _playerController.Conditions.IsCollidingLeft)
        {
            return false;
        }
        else if (_playerController.Conditions.IsOnSlope)
        {
            return false;
        }
        else if (DashLeft <= 0)
        {
            return false;
        }

        return true;
    }

    private IEnumerator DashCountdown()
    {
        DashLeft--;
        _finishCooldown = false;
        float timer = 0f;
        Vector2 direction = new Vector2(_horizontalInput, _verticalInput);
        if (direction == Vector2.zero) // If no input, default to right
        {
            direction = new Vector2(transform.localScale.x, 0f);
        }
        _playerController.Conditions.IsDashing = true;
        _playerController.StopGravity();
        _dashTrail.emitting = true;

        while (timer < _dashDuration)
        {
            _playerController.SetForce(_dashPower * direction.normalized);
            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        _playerController.SetHorizontalForce(0f); // Stop the dash
        _dashTrail.emitting = false;
        _playerController.ResumeGravity();
        _playerController.Conditions.IsDashing = false;

        yield return new WaitForSeconds(_dashCooldown);
        _finishCooldown = true;
    }
}
