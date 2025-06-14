using System.Collections;
using System.Collections.Generic;
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

    private float _fireTimer;
    private float _mx;
    private float _my;

    private Vector2 mousePos;

    protected override void InitState()
    {
        base.InitState();
    }

    public override void ExecuteState()
    {
      
    }

    protected override void GetInput()
    {
        _mx = Input.GetAxisRaw("Horizontal");
        _my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90;

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

    private void Shoot()
    {
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }
}
