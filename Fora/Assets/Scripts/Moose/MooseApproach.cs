using UnityEngine;
using System.Collections;

[System.Serializable]
public class MooseWaypoint
{
    public Vector2 offset;
    public float scale = 1f;
    public float rotation = 0f;
    public float duration = 1f;
}

[DisallowMultipleComponent]
public class MooseApproach : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private MooseWaypoint[] waypoints;

    [Header("References")]
    [SerializeField] private Camera2D camera2D;
    [SerializeField] private BossEscapeHandler bossEscapeHandler;
    [SerializeField] private GameObject[] objectsToActivateBeforeEscape;

    [Header("Fade Out Settings")]
    [SerializeField] private GameObject objectToFadeOut;
    [SerializeField] private float fadeOutDuration = 1.5f;

    private Vector2[] worldPositions;
    private Vector2 basePosition;
    private int currentIndex = 0;
    private float elapsed = 0f;

    private void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("[MooseApproach] At least 2 waypoints required.");
            enabled = false;
            return;
        }

        basePosition = transform.position;
        worldPositions = new Vector2[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            worldPositions[i] = basePosition + waypoints[i].offset;
        }

        ApplyWaypoint(0);
    }

    private void Update()
    {
        if (currentIndex >= waypoints.Length - 1)
        {
            StartCoroutine(EscapeSequence());
            enabled = false;
            return;
        }

        float duration = waypoints[currentIndex].duration;
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        transform.position = Vector2.Lerp(worldPositions[currentIndex], worldPositions[currentIndex + 1], t);
        transform.localScale = Vector2.one * Mathf.Lerp(waypoints[currentIndex].scale, waypoints[currentIndex + 1].scale, t);
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(waypoints[currentIndex].rotation, waypoints[currentIndex + 1].rotation, t));

        if (elapsed >= duration)
        {
            elapsed = 0f;
            currentIndex++;

            if (currentIndex == 1 && camera2D != null)
                camera2D.ShakeCamera(1f, 0.35f);
        }
    }

    private IEnumerator EscapeSequence()
    {
        // ✅ Activate objects
        if (objectsToActivateBeforeEscape != null)
        {
            foreach (GameObject obj in objectsToActivateBeforeEscape)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        if (bossEscapeHandler != null && camera2D != null)
            bossEscapeHandler.ShakeCameraAfterDelay(camera2D, 5f, 60f, 0.12f);

        yield return new WaitForSeconds(5f); // wait before fade starts

        if (objectToFadeOut != null && objectToFadeOut.TryGetComponent(out FadeEffect fade))
        {
            fade.FadeOut(fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
            objectToFadeOut.SetActive(false);
        }

        // ✅ CALL THE COUNTDOWN TIMER HERE
        if (CountdownTimerUI.Instance != null)
        {
            CountdownTimerUI.Instance.StartTimer(60f, new Vector2(0f, 0f), new Vector2(700f, 420f), 3f);
        }
        else
        {
            Debug.LogWarning("[MooseApproach] CountdownTimerUI not found in scene.");
        }

        Destroy(gameObject);
    }


    private void ApplyWaypoint(int index)
    {
        transform.position = worldPositions[index];
        transform.localScale = Vector2.one * waypoints[index].scale;
        transform.rotation = Quaternion.Euler(0f, 0f, waypoints[index].rotation);
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Vector2 origin = Application.isPlaying ? basePosition : (Vector2)transform.position;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector2 pos = origin + waypoints[i].offset;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pos, 0.1f);

            if (i < waypoints.Length - 1)
            {
                Vector2 next = origin + waypoints[i + 1].offset;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(pos, next);
            }

#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos + Vector2.up * 0.2f, $"WP {i}");
#endif
        }
    }
}
