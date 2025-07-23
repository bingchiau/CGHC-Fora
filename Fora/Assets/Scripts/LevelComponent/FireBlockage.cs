using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBlockage : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Transform _nextSpawnPoint;

    private float _health;
    private int _displayHealth;
    private bool _reignite;

    public static Action<Transform> OnBreak;

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        // Check still got enemy alive
        if (_enemies.Count <= 0)
        {
            _reignite = true;
        }

        // Regenerate health
        if (_health < _maxHealth && !_reignite)
        {
            _health += 5 * Time.deltaTime;
        }

        if (_health <= 0)
        {
            OnBreak?.Invoke(_nextSpawnPoint);
            gameObject.SetActive(false);
        }

        _displayHealth = (int)_health;

        _text.text = _displayHealth.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterDroplet"))
        {
            _health -= 20;
        }
    }

    private void RemoveEnemy(GameObject dieEnemy)
    {
        // Check if still have enemies
        for (int enemy = 0; enemy < _enemies.Count; enemy++) 
        {
            if (_enemies[enemy] == dieEnemy)
            {
                _enemies.Remove(_enemies[enemy]);
                _maxHealth -= 40;
                _health = _maxHealth;
            }
        }
    }

    private void OnEnable()
    {
        FadeRemove.OnEnemyDie += RemoveEnemy;
    }

    private void OnDisable()
    {
        FadeRemove.OnEnemyDie -= RemoveEnemy;
    }
}
