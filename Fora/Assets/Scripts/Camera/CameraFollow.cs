using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 2, -10f);
    public float smoothSpeed = 0.125f;

    private Transform player;
    private CharacterMovement playerScript;

    void Start()
    {
        if (CharacterMovement.Instance != null)
        {
            player = CharacterMovement.Instance.transform;
            playerScript = CharacterMovement.Instance;
        }
        else
        {
            Debug.LogError("CharacterMovement singleton not found.");
        }
    }

    void LateUpdate()
    {
        if (player == null || playerScript == null) return;

        float targetY;
        float verticalVelocity = playerScript.GetVerticalVelocity();

        if (playerScript.IsGrounded() || verticalVelocity <= 0f)
        {
            targetY = player.position.y + offset.y;
        }
        else
        {
            targetY = transform.position.y;
        }

        Vector3 targetPosition = new Vector3(player.position.x + offset.x, targetY, player.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
