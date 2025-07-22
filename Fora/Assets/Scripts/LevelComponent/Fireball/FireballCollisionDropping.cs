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
            Vector2 knockbackDir = controller.GetKnockbackDirection();
            pc.SetForce(knockbackDir * controller.KnockbackStrength);
        }
    }
}
