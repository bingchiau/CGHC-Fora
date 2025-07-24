using UnityEngine;

public class L1_PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField, Range(0.05f, 2f)] private float fireRate = 0.25f;

    [Header("Sound")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float shootVolume = 1f;
    [SerializeField] private AudioSource audioSource;

    private float fireCooldown;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (PauseGameManager.Instance != null && PauseGameManager.Instance.IsPaused)
            return;

        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    private void Shoot()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - shootPoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Launch(direction);

        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound, shootVolume);
    }
}
