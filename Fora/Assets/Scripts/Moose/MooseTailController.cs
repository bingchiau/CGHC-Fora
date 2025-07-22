using UnityEngine;

public class MooseTailController : MonoBehaviour
{
    [Header("Tail Segments")]
    public Transform[] segments;

    [Header("Player")]
    public Transform player;

    [Header("Idle Wiggle")]
    public float idleWaveFrequency = 2f;
    public float idleWaveAmplitude = 15f;

    [Header("Sweep Settings")]
    public float sweepAngle = 120f;
    public float sweepSpeed = 200f;

    [Header("Segment Spacing")]
    public float idleSegmentLength = 0.5f;
    public float attackSegmentLength = 1f;

    [Header("Tail Bend")]
    public float sweepWaveAmplitude = 20f;

    [Header("Trigger Radius")]
    public float triggerRadius = 3f;
    public float sweepCooldown = 3f;

    [Header("Sweep Sound")]
    [SerializeField] private AudioClip sweepSound;
    [SerializeField] private float sweepSoundVolume = 1f;

    private AudioSource _audioSource;

    private enum TailState { Idle, Sweep }
    private TailState state = TailState.Idle;

    private float sweepStartAngle;
    private float sweepTargetAngle;
    private float sweepCurrentAngle;

    private float nextSweepTime = 0f;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    void Update()
    {
        bool playerInRange = Vector2.Distance(player.position, transform.position) <= triggerRadius;
        if (playerInRange && state == TailState.Idle && Time.time >= nextSweepTime)
        {
            TriggerTailSweep();
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

            float wave = 0;
            if (state == TailState.Sweep)
                wave = Mathf.Sin(Time.time * 8f + i * 0.5f) * sweepWaveAmplitude / segments.Length;

            angle += wave;

            Vector3 offset = segments[i].right * length;
            pos += offset;
        }
    }

    private void TriggerTailSweep()
    {
        Vector2 dir = player.position - transform.position;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        sweepStartAngle = baseAngle - sweepAngle / 2f;
        sweepTargetAngle = baseAngle + sweepAngle / 2f;
        sweepCurrentAngle = sweepStartAngle;

        state = TailState.Sweep;
        nextSweepTime = Time.time + sweepCooldown;

        // 🔊 Play sweep sound with limited duration
        if (sweepSound != null)
        {
            _audioSource.clip = sweepSound;
            _audioSource.volume = sweepSoundVolume;
            _audioSource.Play();
            StartCoroutine(StopSweepSoundAfterDelay(1f));
        }
    }

    private System.Collections.IEnumerator StopSweepSoundAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        _audioSource.Stop();
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
