using UnityEngine;

public class CameraRotateTrigger : MonoBehaviour
{
    [Header("Camera Rotate Settings")]
    public Camera2D camera2D;       // ✅ Reference to your updated Camera2D script
    public float rotationAngle = 30f;    // Degrees to rotate
    public float rotationDuration = 1.5f; // How long the rotation takes

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            if (camera2D != null)
            {
                camera2D.RotateCameraBy(rotationAngle, rotationDuration);
            }


            Destroy(gameObject);

        }
    }
}
