using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [Header("Lifetime Settings")]
    public float lifetime = 2.5f;

    [Header("Damage Settings")]
    public int damageAmount = 20;

    void Start()
    {
        // Self-destruct after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Try to get PlayerStats from what we hit
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
