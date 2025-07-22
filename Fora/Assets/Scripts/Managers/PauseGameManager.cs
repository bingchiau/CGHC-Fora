using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGameManager : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    [Header("UI Navigation")]
    [SerializeField] private GameObject firstSelectedButton;

    public static PauseGameManager Instance { get; private set; }

    private bool isPaused = false;
    private float bufferTimer = 0f;
    private const float resumeBufferTime = 0.2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (bufferTimer > 0f)
        {
            bufferTimer -= Time.unscaledDeltaTime;
            return;
        }

        HandlePauseInput();
        HandleUIReselect();
    }

    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void HandleUIReselect()
    {
        if (!isPaused || EventSystem.current.currentSelectedGameObject != null)
            return;

        bool usingKeyboard = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                             Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow);

        if (usingKeyboard && pauseMenuUI.activeInHierarchy && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        if (firstSelectedButton != null)
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        bufferTimer = resumeBufferTime;
        AudioListener.pause = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    public bool IsPaused => isPaused;
}
