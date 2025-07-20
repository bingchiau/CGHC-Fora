using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimerUI : MonoBehaviour
{
    public static CountdownTimerUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject timerCanvas; 
    [SerializeField] private TextMeshProUGUI timerText;

    private RectTransform rectTransform;
    private float remainingTime;
    private float moveDuration;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float moveTimer;
    private bool isCounting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (timerText != null)
            rectTransform = timerText.GetComponent<RectTransform>();

        // Hide UI until called
        if (timerCanvas != null)
            timerCanvas.SetActive(false);
    }

    public void StartTimer(float time, Vector2 startPos, Vector2 endPos, float moveTime)
    {
        if (timerCanvas != null && !timerCanvas.activeSelf)
            timerCanvas.SetActive(true);

        remainingTime = time;
        moveDuration = moveTime;
        startPosition = startPos;
        endPosition = endPos;
        moveTimer = 0f;
        isCounting = true;

        if (rectTransform != null)
            rectTransform.anchoredPosition = startPosition;
    }

    private void Update()
    {
        if (!isCounting) return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0f);
        UpdateTimerDisplay(remainingTime);

        if (moveTimer < moveDuration)
        {
            moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(moveTimer / moveDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
        }

        if (remainingTime <= 0f)
        {
            isCounting = false;
            // SceneManager.LoadScene("LoseScene"); 
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int fraction = Mathf.FloorToInt((time * 100f) % 100);
        timerText.text = $"{minutes:00}' {seconds:00}\"{fraction:00}";
    }

    public float RemainingTime => remainingTime;
}
