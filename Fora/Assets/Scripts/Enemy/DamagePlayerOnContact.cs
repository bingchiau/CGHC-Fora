using UnityEngine;

public class DamagePlayerOnContact : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider2D other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}
