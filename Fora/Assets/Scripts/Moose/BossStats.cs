using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 300;
    public int currentHealth;

    public bool isInvincible = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Boss took {amount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Boss healed {amount}. Current Health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Boss has died!");
        // Optional: play death animation, drop loot, disable AI
        GetComponent<MooseBossAI>().enabled = false;
        // You might want to stop movement too:
        GetComponent<BossWaypointMover>().IsPaused = true;
        // Optional: Destroy(gameObject); 
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
