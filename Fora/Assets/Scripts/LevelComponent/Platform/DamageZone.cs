using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damageAmount = 0.2f; // Weight to reduce
    [SerializeField] private float damageInterval = 0.5f; // Interval in seconds

    private PlayerController player;
    private bool isDamaging = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if (player != null && !isDamaging)
            {
                isDamaging = true;
                InvokeRepeating(nameof(ApplyDamage), 0f, damageInterval);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDamaging)
            {
                CancelInvoke(nameof(ApplyDamage));
                isDamaging = false;
                player = null;
            }
        }
    }

    private void ApplyDamage()
    {
        if (player != null)
        {
            player.ReduceWeight(damageAmount);
            // Optional: Add visual or audio feedback here
        }
    }
}
