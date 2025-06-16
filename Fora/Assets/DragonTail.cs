using UnityEngine;

public class DragonTailSweep : MonoBehaviour
{
    [Header("Tail Segments (base to tip)")]
    public Transform[] segments;

    [Header("Sweep Settings")]
    public Transform player;
    public float sweepAngle = 90f; // degrees to sweep
    public float sweepSpeed = 90f;  // degrees per second

    [Header("Segment Spacing")]
    public float idleSegmentDistance = 0.5f;
    public float attackSegmentDistance = 1.0f;
    public float spacingLerpSpeed = 5f;

    [Header("Curvature")]
    [Range(-1f, 1f)]
    public float curvature = 0.5f; // negative = opposite bend

    private enum State { Idle, Sweeping }
    private State state = State.Idle;

    private float currentSweep = 0f;
    private float sweepDirection = -1f; // <-- CCW by default

    private Vector3 sweepCenter;

    private float currentSegmentDistance;

    void Start()
    {
        if (segments.Length > 0)
            sweepCenter = segments[0].position;
        else
            Debug.LogError("Assign tail segments!");

        currentSegmentDistance = idleSegmentDistance;
        UpdateSegmentPositions();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && state == State.Idle)
        {
            state = State.Sweeping;
            currentSweep = 0f;

            Vector2 toPlayer = player.position - sweepCenter;
            float playerAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            float tailAngle = transform.eulerAngles.z;
            float delta = Mathf.DeltaAngle(tailAngle, playerAngle);

            sweepDirection = -1f; // <-- CCW always
        }

        if (state == State.Sweeping)
        {
            float deltaAngle = sweepSpeed * Time.deltaTime * sweepDirection;
            transform.RotateAround(sweepCenter, Vector3.forward, deltaAngle);
            currentSweep += Mathf.Abs(deltaAngle);

            if (currentSweep >= sweepAngle)
            {
                state = State.Idle;
                currentSweep = 0f;
            }
        }

        // Smoothly adjust spacing
        float targetDistance = (state == State.Sweeping) ? attackSegmentDistance : idleSegmentDistance;
        currentSegmentDistance = Mathf.Lerp(currentSegmentDistance, targetDistance, Time.deltaTime * spacingLerpSpeed);

        UpdateSegmentPositions();
    }

    void UpdateSegmentPositions()
    {
        if (segments.Length == 0) return;

        segments[0].position = sweepCenter;

        Vector3 direction = transform.right; // tail forward

        for (int i = 1; i < segments.Length; i++)
        {
            float curvePerSegment = curvature * 20f;

            direction = Quaternion.AngleAxis(curvePerSegment, Vector3.forward) * direction;

            segments[i].position = segments[i - 1].position + direction.normalized * currentSegmentDistance;
        }
    }
}
