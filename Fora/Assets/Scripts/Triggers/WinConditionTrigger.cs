using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class WinConditionTrigger : MonoBehaviour
{
    [Tooltip("The tag of the player object.")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Name of the scene to load when the player wins.")]
    [SerializeField] private string winSceneName = "WinScene";

    private void Awake()
    {
        // Ensure the collider is set as trigger
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("[WinConditionTrigger] 2D Collider was not set as trigger. Enabling it now.");
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("[WinConditionTrigger] Player reached goal. Loading win scene...");
            SceneManager.LoadScene(winSceneName);
        }
    }
}
