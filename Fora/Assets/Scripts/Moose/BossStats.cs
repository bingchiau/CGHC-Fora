using UnityEngine;

[DisallowMultipleComponent]
public class BossStats : MonoBehaviour
{
    public enum BossEndBehavior
    {
        Escape,
        Die
    }

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private bool isInvincible = false;
    private int currentHealth;

    [Header("End Behavior")]
    [SerializeField] private BossEndBehavior endBehavior = BossEndBehavior.Escape;

    [Tooltip("Escape will trigger when health drops to or below this threshold. Not used for Die.")]
    [SerializeField] private int escapeThreshold = 20;

    [Header("Escape Settings")]
    [SerializeField] private GameObject bossEscapeScene;
    [SerializeField] private float escapeDelay = 2f;

    [Header("Death Settings")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private BossDeathHandler deathHandler; // ✅ External handler

    private bool hasTriggeredEnd = false;

    private void Awake() => currentHealth = maxHealth;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[BossStats] DEV: Forced end behavior (K key).");
            currentHealth = (endBehavior == BossEndBehavior.Escape) ? escapeThreshold : 0;
            EvaluateEndCondition();
        }
    }
#endif

    public void TakeDamage(int amount)
    {
        if (isInvincible || currentHealth <= 0 || hasTriggeredEnd) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"[BossStats] Took {amount} damage. HP: {currentHealth}");

        EvaluateEndCondition();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"[BossStats] Healed {amount}. HP: {currentHealth}");
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        hasTriggeredEnd = false;
    }

    private void EvaluateEndCondition()
    {
        switch (endBehavior)
        {
            case BossEndBehavior.Escape:
                if (currentHealth <= escapeThreshold)
                    TriggerEscape();
                break;

            case BossEndBehavior.Die:
                if (currentHealth <= 0)
                    TriggerDeath();
                break;
        }
    }

    private void TriggerEscape()
    {
        if (hasTriggeredEnd) return;
        hasTriggeredEnd = true;

        Debug.Log("[BossStats] Boss is escaping.");
        if (FindObjectOfType<BossEscapeHandler>() is BossEscapeHandler handler)
        {
            handler.HandleBossEscape(gameObject, bossEscapeScene, escapeDelay);
        }
    }

    private void TriggerDeath()
    {
        if (hasTriggeredEnd) return;
        hasTriggeredEnd = true;

        Debug.Log("[BossStats] Boss is dying.");

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (deathHandler != null)
            deathHandler.HandleBossDeath();

        if (destroyOnDeath)
            Destroy(gameObject);
    }
}
