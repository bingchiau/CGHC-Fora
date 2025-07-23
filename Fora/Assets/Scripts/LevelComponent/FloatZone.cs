using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatZone : MonoBehaviour
{
    [Header("Wind Settings")]
    public float windForce = 30f;
    public float fallSpeedThreshold = -5f;

    private PlayerController playerControllerInside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            playerControllerInside = controller;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerControllerInside != null && other.gameObject == playerControllerInside.gameObject)
        {
            playerControllerInside = null;
        }
    }

    private void Update()
    {
        if (playerControllerInside == null) return;

        Vector2 playerVelocity = playerControllerInside.Force;
        float forceToApply = windForce;

        playerControllerInside.SetVerticalForce(forceToApply);
    }
}
