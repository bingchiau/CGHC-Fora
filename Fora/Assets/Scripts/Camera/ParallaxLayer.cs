using UnityEngine;

/// <summary>
/// Represents a single parallax layer that moves at a specific factor relative to camera.
/// </summary>
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("Objects further in the background should have more positive values.\nObjects closer to the foreground should have more negative values.")]
    [Range(-1f, 1f)]
    public float parallaxFactor = 0.2f;

    /// <summary>
    /// Moves the layer horizontally based on camera delta and parallax factor.
    /// </summary>
    /// <param name="delta">Horizontal delta movement from the camera</param>
    public void Move(float delta)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.x -= delta * parallaxFactor;
        transform.localPosition = newPosition;
    }
}
