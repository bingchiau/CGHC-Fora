using System.Collections;
using UnityEngine;

public class MovingPlatform2D : MonoBehaviour
{
    [Header("Path (relative to object start position)")]
    [Tooltip("Points relative to the object’s starting position in the scene.")]
    [SerializeField] private Vector2[] relativePathPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private Vector3[] worldPoints;
    private int currentIndex = 0;
    private bool isWaiting = false;

    // This will store the original position permanently
    private Vector3 storedStartPosition;

    private PlayerController customPlayer;

    private Vector3 lastPos;


    private void Awake()
    {
        // Save the platform's position BEFORE any movement
        storedStartPosition = transform.position;
    }

    private void Start()
    {
        // Convert relative offsets to world-space points
        worldPoints = new Vector3[relativePathPoints.Length];
        for (int i = 0; i < relativePathPoints.Length; i++)
        {
            worldPoints[i] = storedStartPosition + (Vector3)relativePathPoints[i];
        }

        // Move to the first point immediately
        transform.position = worldPoints[0];
        currentIndex = 1;
    }

    private void Update()
    {
        if (worldPoints == null || isWaiting || worldPoints.Length < 2) return;
        MoveToTarget();
    }

    private void LateUpdate()
    {
        if (customPlayer != null)
        {
            Vector3 delta = transform.position - lastPos;
            customPlayer.ApplyPlatformOffset(delta);
        }

        lastPos = transform.position;
    }

    private void MoveToTarget()
    {
        Vector3 target = worldPoints[currentIndex];
        Vector3 current = transform.position;
        Vector3 direction = (target - current).normalized;
        float distance = Vector3.Distance(current, target);

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (distance < 0.05f)
        {
            transform.position = target;
            StartCoroutine(WaitBeforeNextPoint());
        }
    }

    private IEnumerator WaitBeforeNextPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentIndex = (currentIndex + 1) % worldPoints.Length;
        isWaiting = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (relativePathPoints == null || relativePathPoints.Length == 0) return;

        // Use current position in editor as origin
        Vector3 origin = Application.isPlaying ? storedStartPosition : transform.position;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < relativePathPoints.Length; i++)
        {
            Vector3 worldPoint = origin + (Vector3)relativePathPoints[i];
            Gizmos.DrawSphere(worldPoint, 0.15f);

            if (i > 0)
            {
                Vector3 prev = origin + (Vector3)relativePathPoints[i - 1];
                Gizmos.DrawLine(prev, worldPoint);
            }
        }

        // Optional loop line
        if (relativePathPoints.Length > 1)
        {
            Vector3 first = origin + (Vector3)relativePathPoints[0];
            Vector3 last = origin + (Vector3)relativePathPoints[^1];
            Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
            Gizmos.DrawLine(last, first);
        }
    }

    // PLAYER FOLLOW PLATFORM
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            customPlayer = other.GetComponentInParent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            customPlayer = null;
        }
    }
}