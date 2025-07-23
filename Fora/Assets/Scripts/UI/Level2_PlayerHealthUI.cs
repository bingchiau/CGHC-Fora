using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2_PlayerHealthUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI _health;

    [SerializeField] private PlayerStats _playerStats;
    private GameObject _currentPlayer;

    private void Update()
    {
        // Re-acquire if null or if player object changed
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != _currentPlayer)
        {
            _currentPlayer = player;
            if (_currentPlayer != null)
                _playerStats = _currentPlayer.GetComponent<PlayerStats>();
        }

        if (_playerStats != null)
        {
            int currentHealth = _playerStats.CurrentHealth;
            _health.text = "Health " + currentHealth.ToString();
        }
    }
}
