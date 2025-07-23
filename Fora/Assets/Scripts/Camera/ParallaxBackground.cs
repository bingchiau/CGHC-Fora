using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all parallax layers and responds to camera movement updates.
/// </summary>
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("Reference to the ParallaxCamera that triggers updates.")]
    public ParallaxCamera parallaxCamera;

    private readonly List<ParallaxLayer> _parallaxLayers = new();

    void Start()
    {
        // Auto-find main camera if none assigned
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += MoveLayers;

        CacheParallaxLayers();
    }

    /// <summary>
    /// Collects all child objects with ParallaxLayer components.
    /// </summary>
    private void CacheParallaxLayers()
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
    /// Moves each layer based on the camera delta.
    /// </summary>
    private void MoveLayers(float delta)
    {
        foreach (ParallaxLayer layer in _parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
