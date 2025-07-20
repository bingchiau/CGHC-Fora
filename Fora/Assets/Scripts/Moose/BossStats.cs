using UnityEngine;

[DisallowMultipleComponent]
public class BossStats : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private bool isInvincible = false;

    [Header("Escape Sequence")]
    [SerializeField] private GameObject bossEscapeScene;
    [SerializeField] private float escapeDelay = 2f;

    private int currentHealth;

    private void Awake() => currentHealth = maxHealth;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[BossStats] DEV: Forced escape (K key).");
            currentHealth = 20;
            Escape();
        }
#endif
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"[BossStats] Took {amount} damage. HP: {currentHealth}");

        if (currentHealth == 20)
            Escape();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"[BossStats] Healed {amount}. HP: {currentHealth}");
    }

    public void ResetHealth() => currentHealth = maxHealth;

    private void Escape()
    {
        Debug.Log("[BossStats] Boss is escaping.");
        if (FindObjectOfType<BossEscapeHandler>() is BossEscapeHandler handler)
        {
            handler.HandleBossEscape(gameObject, bossEscapeScene, escapeDelay);
        }
    }
}
