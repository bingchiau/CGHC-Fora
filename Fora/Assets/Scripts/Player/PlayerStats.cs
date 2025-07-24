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

    [Header("Audio")]
    [SerializeField] private bool useAudioManager = true;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private AudioSource audioSource;

    private float invincibilityTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Animator _animator;
    private bool _isAlive = true;

    public event Action<int> OnHealthChanged;
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
            Debug.LogWarning("PlayerStats: No SpriteRenderer found!");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

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
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || !IsAlive || !CanHit) return;

        if (CompareTag("Player"))
        {
            if (useAudioManager)
                AudioManager.Instance.PlayGetHit();
            else if (hitSound != null && audioSource != null)
                audioSource.PlayOneShot(hitSound, sfxVolume);
        }
        else if (CompareTag("Enemy"))
        {
            if (useAudioManager)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.hitEnemy);
            else if (hitSound != null && audioSource != null)
                audioSource.PlayOneShot(hitSound, sfxVolume);
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        _animator.SetTrigger("isHit");
        NotifyHealthChanged();

        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(StartInvincible());
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
            StartCoroutine(DelaySpawn());
        }
        else if (CompareTag("Player"))
        {
            if (useAudioManager)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.die);
            else if (deathSound != null && audioSource != null)
                audioSource.PlayOneShot(deathSound, sfxVolume);

            StartCoroutine(DelayDie());
        }
    }

    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(1f);
        OnDeath?.Invoke(GetComponent<PlayerMotor>());
    }

    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(0.5f);
        if (_waterBottle != null)
            Instantiate(_waterBottle, transform.position, Quaternion.identity);
    }

    public void AfterHit() => _animator.SetBool("canHit", true);

    public void ForceDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        NotifyHealthChanged();
        if (currentHealth <= 0)
            Die();
    }

    private void NotifyHealthChanged() => OnHealthChanged?.Invoke(currentHealth);
}
