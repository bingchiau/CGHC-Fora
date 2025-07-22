using UnityEngine;

public class L1_PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;      // Your bullet prefab
    [SerializeField] private Transform shootPoint;         // Position where the bullet spawns
    [SerializeField, Range(0.05f, 2f)] private float fireRate = 0.25f;

    private float fireCooldown;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    private void Shoot()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Calculate direction from shoot point to mouse
        Vector2 direction = (mouseWorldPos - shootPoint.position).normalized;

        // Spawn and launch bullet
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Launch(direction);
    }
}
