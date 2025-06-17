using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 300;
    public int currentHealth;

    public bool isInvincible = false;

    [Header("After Death")]
    public GameObject bossDeathScene; // The object to activate
    public float deathSceneDelay = 2f; // Delay before activating and destroying

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // DEV key to force kill
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("DEV: Forced boss death via K key");
            currentHealth = 0;
            Die();
        }
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

        // Use the death handler to finish up
        BossDeathHandler handler = FindObjectOfType<BossDeathHandler>();
        if (handler != null)
        {
            handler.HandleBossDeath(gameObject, bossDeathScene, deathSceneDelay);
        }

    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
