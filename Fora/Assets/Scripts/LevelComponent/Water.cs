using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _weightAdd = 0.8f;
    private PlayerController _playerController;

    private void Update()
    {
        if (_playerController == null)
        {
            return;
        }
        _playerController.AddWeight(_weightAdd);
        UIManager.Instance.UpdateWeight(_playerController.WeightRatio);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            _playerController = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerController = null;
    }
}
