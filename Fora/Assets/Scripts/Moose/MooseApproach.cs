using UnityEngine;

[System.Serializable]
public class MooseWaypoint
{
    public Vector2 offset;   // Relative to base
    public float scale = 1f; // Uniform scale
    public float rotation = 0f; // Z rotation in degrees
    public float duration = 1f; // Time to move to NEXT waypoint
}

public class MooseApproach : MonoBehaviour
{
    [Header("Waypoints")]
    public MooseWaypoint[] waypoints;

    private Vector2[] worldPositions;
    private float elapsed = 0f;
    private int currentIndex = 0;

    private Vector2 basePosition;

    void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("Define at least 2 waypoints!");
            enabled = false;
            return;
        }

        basePosition = transform.position;

        worldPositions = new Vector2[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            worldPositions[i] = basePosition + waypoints[i].offset;
        }

        transform.position = worldPositions[0];
        transform.localScale = Vector2.one * waypoints[0].scale;
        transform.rotation = Quaternion.Euler(0, 0, waypoints[0].rotation);
    }

    void Update()
    {
        if (currentIndex >= waypoints.Length - 1)
        {
            // ✅ If reached final waypoint, destroy the GameObject
            Destroy(gameObject);
            return;
        }

        float stepDuration = waypoints[currentIndex].duration;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / stepDuration);

        transform.position = Vector2.Lerp(worldPositions[currentIndex], worldPositions[currentIndex + 1], t);
        float scale = Mathf.Lerp(waypoints[currentIndex].scale, waypoints[currentIndex + 1].scale, t);
        transform.localScale = Vector2.one * scale;

        float rot = Mathf.LerpAngle(waypoints[currentIndex].rotation, waypoints[currentIndex + 1].rotation, t);
        transform.rotation = Quaternion.Euler(0, 0, rot);

        if (elapsed >= stepDuration)
        {
            elapsed = 0f;
            currentIndex++;
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Vector2 basePos = Application.isPlaying ? basePosition : (Vector2)transform.position;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector2 wpPos = basePos + waypoints[i].offset;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(wpPos, 0.1f);

            if (i < waypoints.Length - 1)
            {
                Vector2 nextPos = basePos + waypoints[i + 1].offset;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(wpPos, nextPos);
            }

#if UNITY_EDITOR
            UnityEditor.Handles.Label(wpPos + Vector2.up * 0.2f, $"WP {i}");
#endif
        }
    }
}
