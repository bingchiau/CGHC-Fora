using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesDamage : MonoBehaviour
{
    private PlayerShoot _playerShoot;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (this.CompareTag("WaterDroplet"))
        {
            _playerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage((int)_playerShoot.damage);
            Destroy(gameObject);
        }
    }
}
