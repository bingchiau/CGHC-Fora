using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Invincibility")]
    public bool isInvincible = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Call this to deal damage to the player
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Player took {amount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Call this to heal the player
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}. Current Health: {currentHealth}");
    }

    // What happens when health reaches zero
    private void Die()
    {
        Debug.Log("Player has died.");
        // Optional: play death animation, reload level, etc.
    }

    // Optional: restore to full
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
