using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string introVideoSceneName = "IntroVideoScene";

    public void StartGame()
    {
        Debug.Log("Loading intro video...");
        SceneManager.LoadScene(introVideoSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
