using UnityEngine;
using System.Collections;

public class MooseBossAI : MonoBehaviour
{
    [Header("References")]
    public BossWaypointMover mover;
    public Transform player;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    [Header("Fireball Attack Settings")]
    public int fireballAttackWaypointIndex = 2;
    public float fireballAttackChance = 0.5f;
    public float fireballAttackDuration = 3f;
    public float fireballFireRate = 0.5f;
    public float fireballSpeed = 5f;

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private float fireballSoundVolume = 1f;

    private AudioSource _audioSource;
    private int previousWaypointIndex = -1;
    private bool isAttacking = false;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isAttacking) return;

        int currentIndex = GetCurrentWaypointIndex();

        if (currentIndex == fireballAttackWaypointIndex && currentIndex != previousWaypointIndex)
        {
            if (Random.value < fireballAttackChance)
            {
                StartCoroutine(FireballAttackCoroutine());
            }
        }

        previousWaypointIndex = currentIndex;
    }

    int GetCurrentWaypointIndex()
    {
        float scaledT = mover.PathProgress * mover.offsets.Length;
        int index = Mathf.FloorToInt(scaledT) % mover.offsets.Length;
        return index;
    }

    IEnumerator FireballAttackCoroutine()
    {
        isAttacking = true;
        mover.IsPaused = true;

        float elapsed = 0f;
        while (elapsed < fireballAttackDuration)
        {
            ShootFireball();
            yield return new WaitForSeconds(fireballFireRate);
            elapsed += fireballFireRate;
        }

        mover.IsPaused = false;
        isAttacking = false;
    }

    void ShootFireball()
    {
        if (player == null || fireballPrefab == null || fireballSpawnPoint == null)
            return;

        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

        Vector2 direction = (player.position - fireballSpawnPoint.position).normalized;

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * fireballSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (fireballSound != null)
            _audioSource.PlayOneShot(fireballSound, fireballSoundVolume);
    }
}
