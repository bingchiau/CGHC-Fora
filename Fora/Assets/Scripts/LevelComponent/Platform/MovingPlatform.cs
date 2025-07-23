using UnityEngine;

public class MovingPlatform2D : MonoBehaviour
{
    [Header("Path (relative to object start position)")]
    [Tooltip("Points relative to the object’s starting position in the scene.")]
    [SerializeField] private Vector2[] relativePathPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 0f;

    private Vector3[] worldPoints;
    private int currentIndex = 1;
    private bool isActivated = false;
    private bool isWaiting = false;
    private bool isReturningToStart = false;

    private Transform passenger;
    private Transform triggerReference;
    private Vector3 lastTriggerPos;

    private void Awake()
    {
        worldPoints = new Vector3[relativePathPoints.Length];
        for (int i = 0; i < relativePathPoints.Length; i++)
        {
            worldPoints[i] = transform.position + (Vector3)relativePathPoints[i];
        }
    }

    private void Update()
    {
        if (worldPoints == null || worldPoints.Length < 2 || isWaiting)
            return;

        if (isReturningToStart)
        {
            ReturnToStart();
        }
        else if (isActivated)
        {
            MoveToTarget();
        }

        HandlePassengerMovement();
    }

    private void MoveToTarget()
    {
        Vector3 target = worldPoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;
            currentIndex = (currentIndex + 1) % worldPoints.Length;

            if (waitTime > 0f)
                StartCoroutine(WaitBeforeNextMove());
        }
    }

    private System.Collections.IEnumerator WaitBeforeNextMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    private void ReturnToStart()
    {
        Vector3 start = worldPoints[0];
        transform.position = Vector3.MoveTowards(transform.position, start, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, start) < 0.05f)
        {
            transform.position = start;
            isReturningToStart = false;
            isActivated = false;
            currentIndex = 1;
        }
    }

    private void HandlePassengerMovement()
    {
        if (passenger != null && triggerReference != null)
        {
            Vector3 triggerDelta = triggerReference.position - lastTriggerPos;
            passenger.position += triggerDelta;
            lastTriggerPos = triggerReference.position;
        }
    }

    public void ActivatePlatform(Collider2D player, Transform trigger)
    {
        isActivated = true;
        isReturningToStart = false;

        passenger = player.transform;
        triggerReference = trigger;
        lastTriggerPos = trigger.position;
    }

    public void DeactivatePlatform(Collider2D player)
    {
        if (passenger == player.transform)
        {
            passenger = null;
            triggerReference = null;
        }

        isReturningToStart = true;
    }
}
