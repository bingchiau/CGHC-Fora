using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ActivationPlate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float activationWeightThreshold = 2f;
    [SerializeField] private Animator plateAnimator;

    private bool isPlayerOnPlate = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            float playerWeight = player.WeightRatio * 5f;

            if (playerWeight >= activationWeightThreshold)
            {
                plateAnimator.SetBool("Activate", true);
                isPlayerOnPlate = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isPlayerOnPlate)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                float playerWeight = player.WeightRatio * 5f;

                if (playerWeight >= activationWeightThreshold)
                {
                    plateAnimator.SetBool("Activate", true);
                    isPlayerOnPlate = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && isPlayerOnPlate)
        {
            plateAnimator.SetBool("Activate", false);
            isPlayerOnPlate = false;
        }
    }
}
