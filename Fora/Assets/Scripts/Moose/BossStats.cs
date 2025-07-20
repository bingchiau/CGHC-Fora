using UnityEngine;

[DisallowMultipleComponent]
public class BossStats : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private bool isInvincible = false;

    [Header("After Death")]
    [SerializeField] private GameObject bossDeathScene;
    [SerializeField] private float deathSceneDelay = 2f;

    private int currentHealth;

    private void Awake() => currentHealth = maxHealth;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[BossStats] DEV: Forced death.");
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"[BossStats] Took {amount} damage. HP: {currentHealth}");

        if (currentHealth == 0) Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"[BossStats] Healed {amount}. HP: {currentHealth}");
    }

    public void ResetHealth() => currentHealth = maxHealth;

    private void Die()
    {
        Debug.Log("[BossStats] Boss has died.");
        if (FindObjectOfType<BossDeathHandler>() is BossDeathHandler handler)
        {
            handler.HandleBossDeath(gameObject, bossDeathScene, deathSceneDelay);
        }
    }
}