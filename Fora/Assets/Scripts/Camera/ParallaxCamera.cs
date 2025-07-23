using UnityEngine;

/// <summary>
/// Detects camera movement and broadcasts horizontal delta to listeners (e.g., ParallaxBackground).
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

        // Only trigger if there's horizontal movement
        if (!Mathf.Approximately(currentX, _previousX))
        {
            float delta = _previousX - currentX;

            onCameraTranslate?.Invoke(delta); // Broadcast delta to all listeners

            _previousX = currentX;
        }
    }
}
