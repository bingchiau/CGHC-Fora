using UnityEngine;

public class DroppingFireball : MonoBehaviour
{
    public Vector3 localStartOffset = Vector3.zero;
    public Vector3 localEndOffset = new Vector3(0, 0, 5);
    public float speed = 1f;
    public float delayBeforeRespawn = 1f;
    public float knockbackStrength = 20f;

    private Vector3 _origin;
    private float journeyLength;
    private float startTime;
    private bool isMoving = true;

    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider;

    private Vector3 StartPosition => _origin + localStartOffset;
    private Vector3 EndPosition => _origin + localEndOffset;

    void Start()
    {
        _origin = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<Collider2D>();

        ResetToStart();
    }

    void ResetToStart()
    {
        transform.position = StartPosition;
        startTime = Time.time;
        journeyLength = Vector3.Distance(StartPosition, EndPosition);

        // Reactivate visibility and collision
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (objectCollider != null) objectCollider.enabled = true;

        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        float distCovered = (Time.time - startTime) * speed;
        float fraction = distCovered / journeyLength;

        transform.position = Vector3.Lerp(StartPosition, EndPosition, fraction);

        if (fraction >= 1f)
        {
            StartCoroutine(ResetThenWait());
        }
    }

    private System.Collections.IEnumerator ResetThenWait()
    {
        isMoving = false;

        // Disable visuals and collision
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (objectCollider != null) objectCollider.enabled = false;

        // Teleport to start right away
        transform.position = StartPosition;

        yield return new WaitForSeconds(delayBeforeRespawn);
        ResetToStart();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player, resetting and applying knockback.");
            StopAllCoroutines();
            StartCoroutine(ResetThenWait());

            DamagePlayer(other.gameObject);
        }
    }

    private void DamagePlayer(GameObject player)
    {
        // TO-DO: Player damage
        Debug.Log("TODO: Custom damage logic here.");

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            Vector2 knockbackDir = (EndPosition - StartPosition).normalized;
            knockbackDir = new Vector2(knockbackDir.x, knockbackDir.y); // Only XY

            pc.SetForce(knockbackDir * knockbackStrength);
        }
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
