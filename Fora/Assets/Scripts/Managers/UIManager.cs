using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string weightBarName = "WeightBar";
    [SerializeField] private Image _weightImage;
    [SerializeField] private Color[] _color;

    private int _currentColorIndex = 0;
    private int _targetColorIndex = 1;

    private float _weightRatio;
    private bool _initialized = false;
    [SerializeField] private GameObject _currentPlayer;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != _currentPlayer)
        {
            _currentPlayer = player;
            _initialized = false;
        }

        if (!_initialized)
            TryAssignWeightImage();

        if (_weightImage != null)
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

    private void TryAssignWeightImage()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null) return;

        Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);
        Transform weightBarTransform = allChildren.FirstOrDefault(t => t.name == weightBarName);

        if (weightBarTransform != null)
        {
            _weightImage = weightBarTransform.GetComponent<Image>();
            _initialized = _weightImage != null;
        }
    }
}
