using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesDamage : MonoBehaviour
{
    public int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit: " + collision.name);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(Damage);
            DestroyThis();
            return;
        }

        // Check if we hit a boss
        BossStats boss = collision.GetComponent<BossStats>();
        if (boss != null)
        {
            boss.TakeDamage(Damage);
            DestroyThis();
            return;
        }
    }

    public virtual void DestroyThis()
    {
        Destroy(gameObject);
    }
}
