using UnityEngine;

/// <summary>
/// Represents a single background layer with a unique parallax factor.
/// Moves horizontally relative to the camera delta.
/// </summary>
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("Determines how much this layer moves relative to the camera. 0 = static, 1 = full camera movement.")]
    public float parallaxFactor = 0.5f;

    /// <summary>
    /// Moves the layer based on the camera delta and parallax factor.
    /// </summary>
    public void Move(float delta)
    {
        Vector3 pos = transform.localPosition;
        pos.x -= delta * parallaxFactor;
        transform.localPosition = pos;
    }
}
