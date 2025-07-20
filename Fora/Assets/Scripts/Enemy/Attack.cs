using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int _attackDamaged;

    private Collider2D _col;
    private Vector2 _knockback;

    public static Action<Vector2> OnHit;

    private void Start()
    {
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(_attackDamaged);
            //OnHit?.Invoke(_knockback);
        }
    }
}
