using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RelicPickups : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip pickupSound;
    [Range(0f, 1f)][SerializeField] private float soundVolume = 1f;

    private bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp || !other.CompareTag(playerTag)) return;

        pickedUp = true;

        Debug.Log("[GemPickup] Player picked up the gem.");

        // Optional sound effect
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
        }

        // Trigger win screen or placeholder
        TriggerWinScreen();

        // Disable the gem visually and physically
        gameObject.SetActive(false);
    }

    private void TriggerWinScreen()
    {
        // TODO: Replace with actual win screen logic
        Debug.Log("[GemPickup] WIN screen should appear now!");
        // Example placeholder: pause the game
        Time.timeScale = 0f;

        // Later you could do:
        // WinScreenUI.Instance.Show();
        // SceneManager.LoadScene("WinScene");
    }
}
