using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class ScenePortal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private string targetSceneName;

    [Tooltip("Optional delay before loading the new scene.")]
    [SerializeField] private float transitionDelay = 0f;

    [Tooltip("Tag used to identify the player.")]
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered || !other.CompareTag(playerTag)) return;

        hasTriggered = true;
        Debug.Log($"[ScenePortal] Player entered portal. Loading scene: {targetSceneName}");

        if (transitionDelay > 0f)
            Invoke(nameof(LoadScene), transitionDelay);
        else
            LoadScene();
    }

    private void LoadScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("[ScenePortal] Target scene name not set.");
            return;
        }

        SceneManager.LoadScene(targetSceneName);
    }
}
