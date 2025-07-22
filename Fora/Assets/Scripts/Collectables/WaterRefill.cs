using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRefill : MonoBehaviour
{
    [SerializeField] private float _refillAmount = 1.4f;
    private PlayerController _playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerController = collision.gameObject.GetComponent<PlayerController>();

            if (_playerController != null)
            {
                _playerController.AddWeight_oneTime(_refillAmount);
            }
            gameObject.SetActive(false);
        }
    }
}
