using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _firstLevelSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private List<GameObject> _area1;
    [SerializeField] private List<GameObject> _area2;
    [SerializeField] private List<GameObject> _area3;
    [SerializeField] private List<GameObject> _area4;
    [SerializeField] private List<GameObject> _area5;
    [SerializeField] private List<GameObject> _area6;
    [SerializeField] private List<GameObject> _area7;
    [SerializeField] private List<GameObject> _area8;

    // Dictionary to easy access all enemies
    Dictionary<int, List<GameObject>> areas = new Dictionary<int, List<GameObject>>();

    private int _currentArea;
    private GameObject _currentPlayer;
    private Transform _currentSpawnPoint;

    public static Action<PlayerMotor> OnRevive;

    private void Start()
    {
        _currentSpawnPoint = _firstLevelSpawnPoint;
        _currentArea = 1;

        // Add area to dictionary
        areas.Add(1, _area1);
        areas.Add(2, _area2);
        areas.Add(3, _area3);
        areas.Add(4, _area4);
        areas.Add(5, _area5);
        areas.Add(6, _area6);
        areas.Add(7, _area7);
        areas.Add(8, _area8);
    }

    private void Update()
    {
        //...
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
        RevivePlayer();

        for (int i = 0; i < areas[_currentArea].Count; i++)
        {
            areas[_currentArea][i].SetActive(true);
            areas[_currentArea][i].GetComponent<PlayerStats>().ResetHealth();
        }
    }

    private void GetCurrentDieLevel(Transform spawnPoint, int area_enemy)
    {
        _currentSpawnPoint = spawnPoint;
        _currentArea = area_enemy;
    }

    public void SetNewSpawnPoint(Transform spawnPoint, int area_enemy)
    {
        _currentSpawnPoint = spawnPoint;
        _currentArea = area_enemy;
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
