using System;
using System.Collections;
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
    public float invincibilityDuration = 2f;
    public float flashFrequency = 10f;

    [Header("Drops")]
    [SerializeField] private GameObject _waterBottle;

    private float invincibilityTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Animator _animator;

    private bool _isAlive = true;

    public event System.Action<int> OnHealthChanged;

    public static Action<PlayerMotor> OnDeath;

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            _animator.SetBool("isAlive", value);
        }
    }

    public bool CanHit => _animator.GetBool("canHit");

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("PlayerStats: No SpriteRenderer found! Flash will not work.");
        }

        ResetHealth();
    }

    private void Update()
    {
        if (!isInvincible) return;

        invincibilityTimer -= Time.deltaTime;
        _animator.enabled = false;

        if (spriteRenderer != null)
        {
            float wave = Mathf.Sin(Time.time * flashFrequency * Mathf.PI * 2);
            float alpha = Mathf.Abs(wave);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
        }

        if (invincibilityTimer <= 0f)
        {
            isInvincible = false;
            _animator.enabled = true;
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;
        }
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || !IsAlive || !CanHit) return;

        if (CompareTag("Player"))
        {
            AudioManager.Instance.PlayGetHit();
        }
        else if (CompareTag("Enemy"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.hitEnemy);
        }
            currentHealth = Mathf.Max(currentHealth - amount, 0);
        _animator.SetTrigger("isHit");
        Debug.Log($"{gameObject.name} took {amount} damage. Current Health: {currentHealth}");

        NotifyHealthChanged();


        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(StartInvincible());
        }
    }

    private IEnumerator StartInvincible()
    {
        yield return new WaitForSeconds(0.7f);
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;  
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        NotifyHealthChanged();
        Debug.Log($"{gameObject.name} healed {amount}. Current Health: {currentHealth}");
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsAlive = true; 
        NotifyHealthChanged();
    }

    private void Die()
    {
        IsAlive = false;
        if (CompareTag("Enemy"))
        {
            Debug.Log($"Enemy '{name}' has died.");
            StartCoroutine(DelaySpawn());
        }
        else if (CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.die);
            StartCoroutine(DelayDie());
        }
    }

    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(1f);
        OnDeath?.Invoke(gameObject.GetComponent<PlayerMotor>());
    }

    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(0.5f);
        if (_waterBottle != null)
            Instantiate(_waterBottle, transform.position, Quaternion.identity);
    }

    public void AfterHit()
    {
        _animator.SetBool("canHit", true);
    }

    // Alan change
    public void ForceDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"{gameObject.name} took {amount} fire damage. Current Health: {currentHealth}");

        NotifyHealthChanged();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
