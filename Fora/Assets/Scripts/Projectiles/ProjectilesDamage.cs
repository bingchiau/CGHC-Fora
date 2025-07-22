using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesDamage : MonoBehaviour
{
    public int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(Damage);
            DestroyThis();
        }
    }

    public virtual void DestroyThis()
    {
        Destroy(gameObject);
    }
}
