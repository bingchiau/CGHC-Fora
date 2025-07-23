using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _firstLevelSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    private GameObject _currentPlayer;
    private Transform _currentSpawnPoint;
    private GameObject _lastLevelFire;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public static Action<PlayerMotor> OnRevive;

    private void Start()
    {
        _currentSpawnPoint = _firstLevelSpawnPoint;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RevivePlayer();
        }
    }

    // Revives our player
    private void RevivePlayer()
    {
        if (_playerPrefab != null)
        {
            _currentPlayer = Instantiate(_playerPrefab, _currentSpawnPoint.position, Quaternion.identity);
            OnRevive?.Invoke(_currentPlayer.GetComponent<PlayerMotor>());
            _currentPlayer.GetComponent<PlayerStats>().ResetHealth();
        }
    }

    private void PlayerDeath(PlayerMotor player)
    {
        //_currentPlayer = player;
        //_animator = player.GetComponent<Animator>();
        //_spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    private void GetCurrentDieLevel(Transform spawnPoint)
    {
        _currentSpawnPoint = spawnPoint;
    }

    private void OnEnable()
    {
        PlayerStats.OnDeath += PlayerDeath;
        FireBlockage.OnBreak += GetCurrentDieLevel;
    }

    private void OnDisable()
    {
        PlayerStats.OnDeath -= PlayerDeath;
        FireBlockage.OnBreak -= GetCurrentDieLevel;
    }

}
