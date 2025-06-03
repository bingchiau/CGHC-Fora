using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float _dashPower = 20f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;
    [SerializeField] private TrailRenderer _dashTrail;

    private bool canDash = true;

    protected override void InitState()
    {
        base.InitState();
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
        if (Input.GetKeyDown(KeyCode.J) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        _playerController.Conditions.IsDashing = true;
        _playerController.StopGravity();
        _playerController.ApplyDash(_dashPower);
        _dashTrail.emitting = true;
        yield return new WaitForSeconds(_dashDuration);
        _dashTrail.emitting = false;
        _playerController.ResumeGravity();
        _playerController.Conditions.IsDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        canDash = true;
    }
}
