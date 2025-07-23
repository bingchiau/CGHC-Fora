using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollisionDropping : MonoBehaviour
{
    private DroppingFireball controller;

    public void SetController(DroppingFireball controller)
    {
        this.controller = controller;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (controller == null) return;

        if (other.CompareTag("Player")) // Hit PLAYER
        {
            Debug.Log("Hit player from prefab. Destroying and applying knockback.");
            DamagePlayer(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 8) // Hit MAP
        {
            Destroy(gameObject);
        }
    }

    private void DamagePlayer(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            Vector2 knockbackDir = -controller.GetKnockbackDirection();
            knockbackDir.y = Mathf.Abs(knockbackDir.y); // Ensure positive upward Y
            knockbackDir += Vector2.up * 0.5f;          // Optional boost
            pc.SetForce(knockbackDir.normalized * controller.KnockbackStrength);
        }

        PlayerStats ps = player.GetComponent<PlayerStats>();
        if (ps != null)
        {
            ps.TakeDamage(controller.DamageAmount);
        }
    }
}
