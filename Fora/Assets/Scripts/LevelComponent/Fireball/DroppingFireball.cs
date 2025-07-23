using UnityEngine;

public class DroppingFireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Vector3 localStartOffset = Vector3.zero;
    [SerializeField] private Vector3 localEndOffset = new Vector3(0, 0, 5);
    [SerializeField] private float speed = 1f;
    [SerializeField] private float spawnCycleTime = 1f;
    [SerializeField] private float knockbackStrength = 20f;
    [SerializeField] private int damageAmount = 1;

    public float KnockbackStrength => knockbackStrength;
    public int DamageAmount => damageAmount;

    private GameObject spawnedFireball;
    private SpriteRenderer fireballRenderer;
    private Collider2D fireballCollider;

    private Vector3 _origin;
    private float journeyLength;
    private float startTime;
    private bool isMoving = false;

    private float spawnTimer = 0f;

    public Vector3 StartPosition => _origin + localStartOffset;
    public Vector3 EndPosition => _origin + localEndOffset;

    void Start()
    {
        _origin = transform.position;
        spawnTimer = 0f;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Always try to spawn if enough time has passed
        if (spawnTimer >= spawnCycleTime)
        {
            SpawnFireball();
            ResetToStart();
            spawnTimer = 0f;
        }

        // Move fireball if it's active
        if (isMoving && spawnedFireball != null)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fraction = distCovered / journeyLength;

            spawnedFireball.transform.position = Vector3.Lerp(StartPosition, EndPosition, fraction);

            if (fraction >= 1f)
            {
                DespawnFireball();
            }
        }

        // Safety check: fireball destroyed during movement (e.g., by player collision)
        if (isMoving && spawnedFireball == null)
        {
            isMoving = false; // Reset state so next spawn works
        }
    }

    private void SpawnFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogError("No fireball prefab assigned!");
            return;
        }

        if (spawnedFireball != null)
            Destroy(spawnedFireball); // Destroy old one if it somehow remains

        Vector3 direction = EndPosition - StartPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 180f; // Flip the angle

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        spawnedFireball = Instantiate(fireballPrefab, StartPosition, rotation, transform);

        fireballRenderer = spawnedFireball.GetComponent<SpriteRenderer>();
        fireballCollider = spawnedFireball.GetComponent<Collider2D>();

        FireballCollisionDropping fbCollision = spawnedFireball.GetComponent<FireballCollisionDropping>();
        if (fbCollision != null)
            fbCollision.SetController(this);
    }

    void ResetToStart()
    {
        if (spawnedFireball == null) return;

        spawnedFireball.transform.position = StartPosition;
        startTime = Time.time;
        journeyLength = Vector3.Distance(StartPosition, EndPosition);

        if (fireballRenderer != null) fireballRenderer.enabled = true;
        if (fireballCollider != null) fireballCollider.enabled = true;

        isMoving = true;
    }

    void DespawnFireball()
    {
        if (spawnedFireball != null)
        {
            Destroy(spawnedFireball);
        }
        isMoving = false;
    }

    public Vector2 GetKnockbackDirection()
    {
        return (EndPosition - StartPosition).normalized;
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = Application.isPlaying ? _origin : transform.position;
        Vector3 s = origin + localStartOffset;
        Vector3 e = origin + localEndOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(s, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(e, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(s, e);
    }
}
