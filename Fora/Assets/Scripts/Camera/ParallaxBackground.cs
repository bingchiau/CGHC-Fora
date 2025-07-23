using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collects and manages all child ParallaxLayer components and moves them based on camera input.
/// </summary>
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("Reference to the camera script that triggers parallax updates.")]
    public ParallaxCamera parallaxCamera;

    private readonly List<ParallaxLayer> _parallaxLayers = new();

    void Start()
    {
        // Auto-assign ParallaxCamera if not set
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += MoveLayers;

        CacheLayers();
    }

    /// <summary>
    /// Gathers all ParallaxLayer components from children.
    /// </summary>
    private void CacheLayers()
    {
        _parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = $"Layer-{i}";
                _parallaxLayers.Add(layer);
            }
        }
    }

    /// <summary>
    /// Moves each parallax layer according to the camera's horizontal movement.
    /// </summary>
    private void MoveLayers(float delta)
    {
        foreach (ParallaxLayer layer in _parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
