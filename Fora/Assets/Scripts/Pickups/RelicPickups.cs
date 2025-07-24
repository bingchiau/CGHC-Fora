using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class RelicPickups : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip pickupSound;
    [Range(0f, 1f)][SerializeField] private float soundVolume = 1f;

    [Header("Scene Settings")]
    [Tooltip("Exact name of the scene to load (must be in Build Settings).")]
    [SerializeField] private string sceneToLoad;

    private bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp || !other.CompareTag(playerTag)) return;

        pickedUp = true;

        Debug.Log("[RelicPickups] Player picked up the relic.");

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
        }

        LoadScene();

        gameObject.SetActive(false);
    }

    private void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("[RelicPickups] No scene name set. Cannot load scene.");
            return;
        }

        Debug.Log($"[RelicPickups] Loading scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
