using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2_PlayerHealthUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private PlayerStats _playerStats;

    private int _displayHealth;

    private void Start()
    {
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        _displayHealth = _playerStats.CurrentHealth;
        _health.text = "Health " + _displayHealth.ToString();
    }
}
