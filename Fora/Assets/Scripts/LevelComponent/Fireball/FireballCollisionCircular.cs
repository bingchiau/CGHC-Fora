using UnityEngine;

public class FireballCollisionCircular : MonoBehaviour
{
    private CircularFireball controller;

    public void SetController(CircularFireball controller)
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
            controller.DespawnAndRespawn();
        }
        else if (other.gameObject.layer == 8) // Hit MAP
        {
            controller.DespawnAndRespawn();
        }
    }

    private void DamagePlayer(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            Vector2 hitPos = player.GetComponent<Collider2D>().ClosestPoint(transform.position);
            Vector2 knockbackDir = controller.GetKnockbackDirection(hitPos);
            pc.SetForce(knockbackDir * controller.KnockbackStrength);
        }
    }
}
