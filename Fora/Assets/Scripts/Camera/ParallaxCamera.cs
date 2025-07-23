using UnityEngine;

/// <summary>
/// Detects horizontal camera movement and notifies subscribed layers with delta movement.
/// </summary>
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float _previousX;

    void Start()
    {
        _previousX = transform.position.x;
    }

    void Update()
    {
        float currentX = transform.position.x;

        // Broadcast delta only when camera moves horizontally
        if (!Mathf.Approximately(currentX, _previousX))
        {
            float delta = _previousX - currentX;
            onCameraTranslate?.Invoke(delta);
            _previousX = currentX;
        }
    }
}
