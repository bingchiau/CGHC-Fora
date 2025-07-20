using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    public int CurrentHealth => currentHealth;


    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Invincibility")]
    public bool isInvincible = false;
    public float invincibilityDuration = 2f; // 2 seconds
    public float flashFrequency = 10f;       // flashes per second

    private float invincibilityTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("PlayerStats: No SpriteRenderer found! Flash will not work.");
        }
    }


    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (spriteRenderer != null)
            {
                // Use a sine wave to animate transparency between white and transparent
                float wave = Mathf.Sin(Time.time * flashFrequency * Mathf.PI * 2);
                float alpha = Mathf.Abs(wave); // oscillates between 0 and 1

                // Flash as white, fading in/out
                spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            }

            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;

                // Restore full opaque normal color
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }

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

        // Start invincibility frames
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}. Current Health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Optional: handle death animation, game over, respawn, etc.
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
