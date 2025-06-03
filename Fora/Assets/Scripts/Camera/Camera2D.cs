using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerMotor _playerToFollow;
    [SerializeField] private bool horizontalFollow = true;
    [SerializeField] private bool verticalFollow = true;

    [Header("Horizontal")]
    [SerializeField][Range(0, 1)] private float horizontalInFluence = 1f;
    [SerializeField] private float horizontalSmoothness = 3f;

    [Header("Vertical")]
    [SerializeField][Range(0, 1)] private float verticalInFluence = 1f;
    [SerializeField] private float verticalSmoothness = 3f;

    // Position of the Target
    public Vector3 TargetPosition { get; private set; }

    // Reference of the Target Position known by the camera
    public Vector3 CameraTargetPosition { get; private set; }

    private float _targetHorizontalSmoothFollow;
    private float _targetVerticalSmoothFollow;

    private void Awake()
    {
        CenterOnTarget(_playerToFollow);
    }

    private void Update()
    {
        MoveCamera();
    }

    // Returns the position of out target
    private Vector3 GetTargetPosition(PlayerMotor player)
    {
        float xPos = 0f;
        float yPos = 0f;

        xPos += (player.transform.position.x) * horizontalInFluence;
        yPos += (player.transform.position.y) * verticalInFluence;

        Vector3 positionTarget = new Vector3(xPos, yPos, transform.position.z);
        return positionTarget;
    }

    // Centers the camera on the target
    private void CenterOnTarget(PlayerMotor player)
    {
        Vector3 targetPosition = GetTargetPosition(player);
        _targetHorizontalSmoothFollow = targetPosition.x;
        _targetVerticalSmoothFollow = targetPosition.y; 

        transform.localPosition = targetPosition;
    }

    // Moves the camera
    private void MoveCamera()
    {
        // Calculate Position
        TargetPosition = GetTargetPosition(_playerToFollow);
        CameraTargetPosition = new Vector3(TargetPosition.x, TargetPosition.y, 0f);

        // Follow on selected axis
        float xPos = horizontalFollow? CameraTargetPosition.x : transform.localPosition.x;
        float yPos = verticalFollow? CameraTargetPosition.y : transform.localPosition.y;

        // Set smooth value
        _targetHorizontalSmoothFollow = Mathf.Lerp(_targetHorizontalSmoothFollow, CameraTargetPosition.x, horizontalSmoothness * Time.deltaTime);
        _targetVerticalSmoothFollow = Mathf.Lerp(_targetVerticalSmoothFollow, CameraTargetPosition.y, verticalSmoothness * Time.deltaTime);

        // Get direction towards target pos
        float xDirection = _targetHorizontalSmoothFollow - transform.localPosition.x;
        float yDirection = _targetVerticalSmoothFollow - transform.localPosition.y;
        Vector3 deltaDirection = new Vector3(xDirection, yDirection, 0f);

        // New position
        Vector3 newCameraPosition = transform.localPosition + deltaDirection;

        // Apply new position
        transform.localPosition = new Vector3(newCameraPosition.x, newCameraPosition.y, transform.localPosition.z);
    }
}
