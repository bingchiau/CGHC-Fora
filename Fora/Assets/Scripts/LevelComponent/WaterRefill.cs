using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRefill : MonoBehaviour
{
    private PlayerController _playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerController = collision.gameObject.GetComponent<PlayerController>();

            if (_playerController != null)
            {
                _playerController.AddWeight_oneTime(2f);
            }
            gameObject.SetActive(false);
        }
    }
}
