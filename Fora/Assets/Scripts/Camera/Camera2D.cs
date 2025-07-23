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

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0f;
    [SerializeField] private float shakeMagnitude = 0.3f;
    [SerializeField] private float shakeFrequency = 1f; // toggles per second

    private float shakeTimer = 0f;
    private float shakeElapsed = 0f;

    [Header("Rotation Settings")]
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float rotationDuration = 0f;
    private float rotationTimer = 0f;
    private bool isRotating = false;

    // Position of the Target
    public Vector3 TargetPosition { get; private set; }
    public Vector3 CameraTargetPosition { get; private set; }

    private float _targetHorizontalSmoothFollow;
    private float _targetVerticalSmoothFollow;

    private void Awake()
    {
        CenterOnTarget(_playerToFollow);
        startRotation = transform.rotation;
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    // Returns the position of our target
    private Vector3 GetTargetPosition(PlayerMotor player)
    {
        if (player == null) return Vector3.zero;
        float xPos = player.transform.position.x * horizontalInFluence;
        float yPos = player.transform.position.y * verticalInFluence;
        return new Vector3(xPos, yPos, transform.position.z);
    }

    // Centers the camera on the target immediately
    private void CenterOnTarget(PlayerMotor player)
    {
        _playerToFollow = player;
        Vector3 targetPosition = GetTargetPosition(player);
        _targetHorizontalSmoothFollow = targetPosition.x;
        _targetVerticalSmoothFollow = targetPosition.y;
        transform.localPosition = targetPosition;
    }

    private void MoveCamera()
    {
        // Compute target follow position
        TargetPosition = GetTargetPosition(_playerToFollow);
        CameraTargetPosition = new Vector3(TargetPosition.x, TargetPosition.y, 0f);

        // Smooth follow
        _targetHorizontalSmoothFollow = Mathf.Lerp(_targetHorizontalSmoothFollow, CameraTargetPosition.x, horizontalSmoothness * Time.deltaTime);
        _targetVerticalSmoothFollow = Mathf.Lerp(_targetVerticalSmoothFollow, CameraTargetPosition.y, verticalSmoothness * Time.deltaTime);

        float xFollow = horizontalFollow ? _targetHorizontalSmoothFollow : transform.localPosition.x;
        float yFollow = verticalFollow ? _targetVerticalSmoothFollow : transform.localPosition.y;

        // Base follow position
        Vector3 followPosition = new Vector3(xFollow, yFollow, transform.localPosition.z);

        // Consistent step shake: toggle pattern
        if (shakeTimer > 0f)
        {
            shakeElapsed += Time.deltaTime;

            float period = 1f / shakeFrequency;
            bool isOffsetPhase = Mathf.FloorToInt(shakeElapsed / (period * 0.5f)) % 2 == 0;

            float fade = shakeTimer / shakeDuration;

            if (isOffsetPhase)
            {
                float offset = shakeMagnitude * fade;
                followPosition.x += offset;
                followPosition.y += offset; // or -offset for other diagonal
            }

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeElapsed = 0f; // reset
        }

        transform.localPosition = followPosition;
    }

    private void RotateCamera()
    {
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            if (t >= 1f)
            {
                isRotating = false;
            }
        }
    }

    // Public method to trigger camera shake
    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimer = duration;
        shakeElapsed = 0f;
    }

    // Public method to rotate camera by a given degrees smoothly
    public void RotateCameraBy(float degrees, float duration)
    {
        startRotation = transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0, 0, degrees);
        rotationDuration = duration;
        rotationTimer = 0f;
        isRotating = true;
    }

    private void OnEnable()
    {
        LevelManager.OnRevive += CenterOnTarget;
    }

    private void OnDisable()
    {
        LevelManager.OnRevive -= CenterOnTarget;
    }
}
