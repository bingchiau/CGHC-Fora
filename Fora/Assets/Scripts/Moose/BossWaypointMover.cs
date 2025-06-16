using UnityEngine;

public class BossWaypointMover_Smooth : MonoBehaviour
{
    [Header("Waypoint Offsets (from start position)")]
    public Vector3[] offsets;

    private Vector3[] waypoints;
    private int currentWaypointIndex = 0;
    private Vector3 startPosition;

    [Header("Movement")]
    public float timeToReachPoint = 2f; // How long to travel from one waypoint to next

    private float progress = 0f;
    private Vector3 previousPoint, nextPoint;

    void Start()
    {
        // Cache start
        startPosition = transform.position;

        // Compute waypoints
        waypoints = new Vector3[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            waypoints[i] = startPosition + offsets[i];
        }

        // Initialize first pair
        previousPoint = transform.position;
        nextPoint = waypoints[currentWaypointIndex];
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        progress += Time.deltaTime / timeToReachPoint;
        float easedProgress = EaseInOut(progress);

        // Interpolate position with easing
        transform.position = Vector3.Lerp(previousPoint, nextPoint, easedProgress);

        // If reached, go to next
        if (progress >= 1f)
        {
            progress = 0f;

            previousPoint = nextPoint;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            nextPoint = waypoints[currentWaypointIndex];
        }
    }

    // Smoothstep easing: slow at start & end, fast in middle
    float EaseInOut(float t)
    {
        return t * t * (3f - 2f * t);  // classic smoothstep
    }

    void OnDrawGizmos()
    {
        if (offsets == null || offsets.Length == 0) return;

        Vector3 basePosition = Application.isPlaying ? startPosition : transform.position;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 point = basePosition + offsets[i];
            Gizmos.DrawSphere(point, 0.2f);

            if (i < offsets.Length - 1)
            {
                Vector3 nextPoint = basePosition + offsets[i + 1];
                Gizmos.DrawLine(point, nextPoint);
            }
        }

        if (offsets.Length > 1)
        {
            Vector3 lastPoint = basePosition + offsets[offsets.Length - 1];
            Vector3 firstPoint = basePosition + offsets[0];
            Gizmos.DrawLine(lastPoint, firstPoint);
        }
    }
}
