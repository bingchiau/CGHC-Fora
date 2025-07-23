using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerDash : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _minDashPower = 5f;
    [SerializeField] private float _maxDashPower = 15f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 0.5f;
    [SerializeField] private int _maxDashes = 1;
    [SerializeField] private TrailRenderer _dashTrail;
    [SerializeField] private GameObject aim;
    [SerializeField] private LayerMask _whatCanBounce;
    
    private bool _finishCooldown = false;
    private bool _canBounce;
    private float _dashPower;

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

        _dashPower = EvaluateWeight(_maxDashPower, _minDashPower);
    }

    protected override void GetInput()
    {
        if (Input.GetMouseButtonDown(1))
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
        StartCoroutine(BounceCooldown());
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

        // check aim direction
        Vector2 direction = aim.transform.up;

        _playerController.Conditions.IsDashing = true;
        _playerController.StopGravity();
        _dashTrail.emitting = true;

        

        while (timer < _dashDuration)
        {
            _playerController.Conditions.IsBouncing = true;
            RaycastHit2D ray = Physics2D.Raycast(_playerController.transform.position, direction, 0.7f, _whatCanBounce);
            Debug.DrawRay(_playerController.transform.position, direction * 0.7f, Color.green);
            if (ray)
            {
                _canBounce = true;    
            }
            
            if (_canBounce)
            {
                Vector2 dir = _playerController.Bounce(direction.normalized);
                _playerController.SetForce(dir * _dashPower);
            }
            else
            {
                _playerController.SetForce(_dashPower * direction.normalized);
            }
            
            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        _canBounce = false;
        _playerController.SetHorizontalForce(0f);
        _playerController.SetVerticalForce(0f);// Stop the dash
        _dashTrail.emitting = false;
        
        _playerController.Conditions.IsDashing = false;
        
        yield return new WaitForSeconds(_dashCooldown);
        _finishCooldown = true;
        _playerController.Conditions.IsBouncing = false;
        _playerController.ResumeGravity();

    }

    private IEnumerator BounceCooldown()
    {

        yield return new WaitForSeconds(_dashCooldown * 2);
        _canBounce = false;
         
    }

    private float EvaluateWeight(float max, float min)
    {

        float dashPower = Mathf.Lerp(max, min, _playerController.WeightRatio);
        return dashPower;
    }

    public void RefillDash()
    {
        DashLeft = _maxDashes;
        _finishCooldown = true;
    }
}
