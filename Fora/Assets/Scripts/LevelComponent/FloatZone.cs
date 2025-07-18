using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatZone : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;

    [Header("Wind Settings")]
    public float windForce = 30f;
    //public float bounceMultiplier = 1.5f;
    public float fallSpeedThreshold = -5f;

    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            playerInside = false;
        }
    }

    private void Update()
    {
        if (!playerInside) return;

        Vector2 playerVelocity = playerController.Force;

        float forceToApply = windForce;

        /*if (playerVelocity.y < fallSpeedThreshold)
        {
            forceToApply *= bounceMultiplier;
        }*/

        playerController.SetVerticalForce(windForce);   // WITHOUT bounceMultiplier


        // Directly apply upward force through your controller
        playerController.SetVerticalForce(forceToApply);
    }

}
