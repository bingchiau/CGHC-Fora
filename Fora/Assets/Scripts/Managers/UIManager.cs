using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Settings")]
    [SerializeField] private Image _weightImage;
    [SerializeField] private Color[] _color;
    [SerializeField] private GameObject[] _playerLifes;

    private int _currentColorIndex = 0;
    private int _targetColorIndex = 1;

    private float _weightRatio;

    private void Update()
    {
        InternalWeightUpdate();
    }

    public void UpdateWeight(float weightRatio)
    {
        _weightRatio = weightRatio;
    }

    private void InternalWeightUpdate()
    {
        _weightImage.fillAmount = Mathf.Lerp(_weightImage.fillAmount, _weightRatio, Time.deltaTime * 10f);

        _weightImage.color = Color.Lerp(_color[_currentColorIndex], _color[_targetColorIndex], _weightImage.fillAmount);
    }

    // Updates the player lifes
    private void OnPlayerLifes(int currentLifes)
    {
        for (int i = 0; i < _playerLifes.Length; i++)
        {
            if (i < currentLifes) // Total is 3, so value = 2
            {
                _playerLifes[i].gameObject.SetActive(true);
            }
            else
            {
                _playerLifes[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        Health.OnLifesChanged += OnPlayerLifes;
    }

    private void OnDisable()
    {
        Health.OnLifesChanged -= OnPlayerLifes;
    }

}
