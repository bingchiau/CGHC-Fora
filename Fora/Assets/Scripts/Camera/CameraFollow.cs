using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public CharacterMovement playerScript; // Drag your Player GameObject here

    private void LateUpdate()
    {
        float targetY;

        // Allow access to vertical movement conditions
        float playerY = player.position.y;
        float verticalVelocity = playerScript.GetVerticalVelocity();

        // Follow Y if grounded or falling, but not while jumping up
        if (playerScript.IsGroundedPublic() || verticalVelocity <= 0f)
        {
            targetY = playerY + offset.y;
        }
        else
        {
            targetY = transform.position.y; // Stay at current Y during jump
        }

        Vector3 targetPosition = new Vector3(player.position.x + offset.x, targetY, player.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}
