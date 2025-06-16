using UnityEngine;

public class BossWaypointMover : MonoBehaviour
{
    [Header("Waypoint Offsets (from start position)")]
    public Vector3[] offsets;

    private Vector3[] waypoints;
    private Vector3 startPosition;

    [Header("Movement")]
    public float timeToCompletePath = 8f; // Total time for full loop
    private float t = 0f; // Overall path progress

    public bool IsPaused { get; set; } = false;

    // Read-only access to t for AI scripts
    public float PathProgress => t;

    void Start()
    {
        startPosition = transform.position;

        // Compute absolute waypoint positions
        waypoints = new Vector3[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            waypoints[i] = startPosition + offsets[i];
        }
    }

    void Update()
    {
        if (waypoints.Length < 2) return;
        if (IsPaused) return;

        // Increase t smoothly
        t += Time.deltaTime / timeToCompletePath;
        if (t > 1f) t -= 1f;

        Vector3 positionOnCurve = GetPointOnClosedCatmullRom(t);
        transform.position = positionOnCurve;
    }

    Vector3 GetPointOnClosedCatmullRom(float t)
    {
        int numPoints = waypoints.Length;

        float scaledT = t * numPoints;
        int i0 = Mathf.FloorToInt(scaledT) % numPoints;
        int i1 = (i0 + 1) % numPoints;
        int iMinus = (i0 - 1 + numPoints) % numPoints;
        int iPlus = (i1 + 1) % numPoints;

        float localT = scaledT - i0;

        return CatmullRom(
            waypoints[iMinus],
            waypoints[i0],
            waypoints[i1],
            waypoints[iPlus],
            localT
        );
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    void OnDrawGizmos()
    {
        if (offsets == null || offsets.Length < 2) return;

        Gizmos.color = Color.yellow;

        Vector3 basePos = Application.isPlaying ? startPosition : transform.position;

        Vector3[] absPoints = new Vector3[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            absPoints[i] = basePos + offsets[i];
        }

        Vector3 prev = absPoints[0];
        for (float t = 0; t < 1f; t += 0.02f)
        {
            Vector3 point = GetPointOnClosedCatmullRomPreview(absPoints, t);
            Gizmos.DrawLine(prev, point);
            prev = point;
        }
        Gizmos.DrawLine(prev, absPoints[0]); // close loop
    }

    Vector3 GetPointOnClosedCatmullRomPreview(Vector3[] pts, float t)
    {
        int numPoints = pts.Length;
        float scaledT = t * numPoints;
        int i0 = Mathf.FloorToInt(scaledT) % numPoints;
        int i1 = (i0 + 1) % numPoints;
        int iMinus = (i0 - 1 + numPoints) % numPoints;
        int iPlus = (i1 + 1) % numPoints;

        float localT = scaledT - i0;
        return CatmullRom(pts[iMinus], pts[i0], pts[i1], pts[iPlus], localT);
    }
}
