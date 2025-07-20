using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Action<int> OnLifesChanged;

    [Header("Settings")]
    [SerializeField] private int lifes = 3;

    private int _maxLifes;
    private int _currentLifes;

    private void Awake()
    {
        _maxLifes = lifes;
    }

    private void Start()
    {
        ResetLife();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LoseLife();
        }
    }

    public void AddLife()
    {
        _currentLifes += 1;
        if (_currentLifes > _maxLifes)
        {
            _currentLifes = _maxLifes;
        }

        UpdateLifesUI();
    }

    public void LoseLife()
    {
        _currentLifes -= 1;
        if (_currentLifes <= 0)
        {
            _currentLifes = 0;
        }

        UpdateLifesUI();
    }

    public void ResetLife()
    {
        _currentLifes = lifes;
        UpdateLifesUI();
    }

    private void UpdateLifesUI()
    {
        OnLifesChanged?.Invoke(_currentLifes);
    }

}
