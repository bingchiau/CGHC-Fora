using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class WinConditionTrigger : MonoBehaviour
{
    [Tooltip("The tag of the player object.")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Name of the scene to load when the player wins.")]
    [SerializeField] private string winSceneName = "WinScene";

    private void Awake()
    {
        // Ensure the collider is set as trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("[WinConditionTrigger] Collider was not set as trigger. Enabling it now.");
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("[WinConditionTrigger] Player reached goal. Loading win scene...");
            SceneManager.LoadScene(winSceneName);
        }
    }
}
