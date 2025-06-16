using UnityEngine;

public class MooseTailController : MonoBehaviour
{
    [Header("Tail Segments")]
    public Transform[] segments; // base to tip

    [Header("Player")]
    public Transform player;

    [Header("Idle Wiggle")]
    public float idleWaveFrequency = 2f;
    public float idleWaveAmplitude = 15f;

    [Header("Sweep Settings")]
    public float sweepAngle = 120f;     // total degrees of sweep
    public float sweepSpeed = 200f;     // degrees per second

    [Header("Segment Spacing")]
    public float idleSegmentLength = 0.5f;
    public float attackSegmentLength = 1f;

    [Header("Tail Bend")]
    public float sweepWaveAmplitude = 20f; // how curvy during sweep

    private enum TailState { Idle, Sweep }
    private TailState state = TailState.Idle;

    private float sweepStartAngle;
    private float sweepTargetAngle;
    private float sweepCurrentAngle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && state == TailState.Idle)
        {
            Vector2 dir = player.position - transform.position;
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            sweepStartAngle = baseAngle - sweepAngle / 2f;
            sweepTargetAngle = baseAngle + sweepAngle / 2f;
            sweepCurrentAngle = sweepStartAngle;

            state = TailState.Sweep;
        }

        switch (state)
        {
            case TailState.Idle: DoIdle(); break;
            case TailState.Sweep: DoSweep(); break;
        }
    }

    void LateUpdate()
    {
        ApplyFK();
    }

    void DoIdle()
    {
        float wiggle = Mathf.Sin(Time.time * idleWaveFrequency) * idleWaveAmplitude;
        sweepCurrentAngle = wiggle;
    }

    void DoSweep()
    {
        sweepCurrentAngle += sweepSpeed * Time.deltaTime;
        if (sweepCurrentAngle >= sweepTargetAngle)
        {
            state = TailState.Idle;
        }
    }

    void ApplyFK()
    {
        Vector3 pos = transform.position;
        float angle = sweepCurrentAngle;

        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].position = pos;
            segments[i].rotation = Quaternion.Euler(0, 0, angle);

            float length = (state == TailState.Idle) ? idleSegmentLength : attackSegmentLength;

            // Add curvature per segment: wave bends it more
            float wave = 0;
            if (state == TailState.Sweep)
                wave = Mathf.Sin(Time.time * 8f + i * 0.5f) * sweepWaveAmplitude / segments.Length;

            angle += wave;

            Vector3 offset = segments[i].right * length;
            pos += offset;
        }
    }
}
