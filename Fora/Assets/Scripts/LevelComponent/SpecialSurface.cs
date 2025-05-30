using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSurface : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float friction = 10f;

    [Header("Movement")]
    [SerializeField] private float horizontalMovement = 2f;

    public float Friction => friction;

    private PlayerController _playerController;

    private void Update()
    {
        if (_playerController == null)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Enter");
            _playerController = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerController = null;
    }
}
