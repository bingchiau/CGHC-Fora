using UnityEngine;

public class PlatformTriggerRelay : MonoBehaviour
{
    [SerializeField] private MovingPlatform2D platform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            platform.ActivatePlatform(other, transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            platform.DeactivatePlatform(other);
        }
    }
}
