using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _dashPower = 30f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;
    [SerializeField] private int _maxDashes = 1;
    [SerializeField] private TrailRenderer _dashTrail;
    
    public int DashLeft { get; private set; }
    protected override void InitState()
    {
        base.InitState();
        DashLeft = _maxDashes;
    }

    public override void ExecuteState()
    {
        if (_playerController.Conditions.IsDashing)
        {
            return;
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

        float timer = 0f;
        float direction = transform.localScale.x;
        _playerController.Conditions.IsDashing = true;
        _playerController.StopGravity();
        _dashTrail.emitting = true;

        while (timer < _dashDuration)
        {
            _playerController.SetHorizontalForce(_dashPower * direction);
            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        _playerController.SetHorizontalForce(0f); // Stop the dash
        _dashTrail.emitting = false;
        _playerController.ResumeGravity();
        _playerController.Conditions.IsDashing = false;

        yield return new WaitForSeconds(_dashCooldown);
        DashLeft++;
    }
}
