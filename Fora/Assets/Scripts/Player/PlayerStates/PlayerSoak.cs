using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoak : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            // Handle player soaking in water
            Debug.Log("Player is soaking in water.");
            // Add any additional logic for soaking here
        }
    }
}
