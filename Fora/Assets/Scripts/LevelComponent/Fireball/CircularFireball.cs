using UnityEngine;

public class CircularFireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float radius = 3f;
    [Tooltip("Speed in revolutions per second (1 = full circle per second)")]
    [SerializeField] private float revolutionsPerSecond = 0.2f;
    [SerializeField] private bool clockwise = true;
    [SerializeField] private float knockbackStrength = 20f;
    [SerializeField] private float respawnDelay = 2f;

    private GameObject spawnedFireball;
    private float angle; // Radians
    private bool isMoving;
    private Vector3 center;

    public float KnockbackStrength => knockbackStrength;

    void Start()
    {
        center = transform.position;
        SpawnFireball();
    }

    void Update()
    {
        if (isMoving && spawnedFireball != null)
        {
            float direction = clockwise ? -1f : 1f;
            float angleDelta = direction * 2 * Mathf.PI * revolutionsPerSecond * Time.deltaTime;

            float previousAngle = angle;
            angle += angleDelta;
            angle %= (2 * Mathf.PI); // Keep angle in range

            Vector3 previousPos = center + new Vector3(Mathf.Cos(previousAngle), Mathf.Sin(previousAngle), 0) * radius;
            Vector3 newPos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            spawnedFireball.transform.position = newPos;

            // Rotate fireball to face movement direction
            Vector3 movementDir = (newPos - previousPos).normalized;
            if (movementDir != Vector3.zero)
            {
                spawnedFireball.transform.right = movementDir;
                spawnedFireball.transform.Rotate(0f, 0f, 180f); // Flip 180 degrees
            }
        }
    }

    public void DespawnAndRespawn()
    {
        if (spawnedFireball != null)
        {
            Destroy(spawnedFireball);
            spawnedFireball = null;
        }

        isMoving = false;
        Invoke(nameof(SpawnFireball), respawnDelay);
    }

    void SpawnFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogError("No fireball prefab assigned!");
            return;
        }

        angle = 0f;
        Vector3 spawnPos = center + new Vector3(radius, 0, 0); // Angle 0 (right)

        spawnedFireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity, transform);

        var fbCollision = spawnedFireball.GetComponent<FireballCollisionCircular>();
        if (fbCollision != null)
            fbCollision.SetController(this);

        isMoving = true;
    }

    public Vector2 GetKnockbackDirection(Vector2 hitPosition)
    {
        return (hitPosition - (Vector2)center).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? center : transform.position, radius);
    }
}
