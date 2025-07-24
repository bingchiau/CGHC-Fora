using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CutsceneEndLoader : MonoBehaviour
{
    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "Level1";

    [Header("Skip Settings")]
    [SerializeField] private KeyCode skipKey = KeyCode.Space;
    [SerializeField] private bool allowSkip = true;

    private VideoPlayer videoPlayer;
    private bool hasLoadedScene = false;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.loopPointReached += OnCutsceneFinished;
    }

    private void Update()
    {
        if (allowSkip && !hasLoadedScene && Input.GetKeyDown(skipKey))
        {
            Debug.Log("Cutscene skipped by player.");
            LoadNextScene();
        }
    }

    private void OnCutsceneFinished(VideoPlayer vp)
    {
        Debug.Log("Cutscene finished.");
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        hasLoadedScene = true;
        SceneManager.LoadScene(nextSceneName);
    }
}
