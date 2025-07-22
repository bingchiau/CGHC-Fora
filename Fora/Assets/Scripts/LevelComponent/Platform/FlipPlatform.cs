using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPlatform : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float flipBackDelay = 2f; // Seconds before flipping back

    private bool isFlipping = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player and not already flipping
        if (!isFlipping && other.CompareTag("Player"))
        {
            // Check if the trigger is the top one
            Vector2 contactPoint = other.ClosestPoint(transform.position);
            bool isTop = contactPoint.y > transform.position.y;

            if (isTop)
            {
                TriggerFlip();
            }
        }
    }

    private void TriggerFlip()
    {
        animator.SetTrigger("Flip");
        isFlipping = true;
        Invoke(nameof(TriggerAltFlip), flipBackDelay);
    }

    private void TriggerAltFlip()
    {
        animator.SetTrigger("FlipAlt");
        isFlipping = false;
    }
}
