using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int _attackDamaged;

    private Rigidbody2D _rb;

    public static Action<Vector2> OnHit;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(_attackDamaged);
            StartCoroutine(HitStun());
        }
    }

    private IEnumerator HitStun()
    {
       Vector2 velocity = _rb.velocity;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        yield return new WaitForSeconds(0.3f);
        _rb.velocity = velocity;
        _rb.gravityScale = 1f;
        
    }
}
