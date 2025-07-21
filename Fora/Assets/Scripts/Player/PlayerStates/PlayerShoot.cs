using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShoot : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;

    private ProjectilesDamage _pDamage;
    private float _damage;
    private float _fireTimer;

    private Vector2 _mousePos;

    protected override void InitState()
    {
        base.InitState();
        _pDamage = projectilePrefab.GetComponent<ProjectilesDamage>();
        _damage = _pDamage.Damage;
    }

    public override void ExecuteState()
    {  
        //...
    }

    protected override void GetInput()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(_mousePos.y - transform.position.y, _mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90;

        if (_playerController.FacingRight)
        {
            aim.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            aim.transform.localRotation = Quaternion.Euler(0, 0, -angle);
        }

        if (Input.GetMouseButton(0) && _fireTimer <= 0)
        {
            Shoot();
            _fireTimer = fireRate;
        }
        else
        {
            _fireTimer -= Time.deltaTime;
        }
    }

    private bool CanShoot()
    {
        if (_playerController.WeightRatio <= 0.2f)
        {
            return false;
        }
        return true;
    }

    private void Shoot()
    {
        if (CanShoot())
        {
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            _playerController.ReduceWeight(_damage / 100);
            UIManager.Instance.UpdateWeight(_playerController.WeightRatio);
        }
    }
}
