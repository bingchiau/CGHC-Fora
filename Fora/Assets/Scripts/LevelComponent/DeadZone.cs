using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private PlayerStats _playerStats;

    public void Die(GameObject player)
    {
        _playerStats = player.GetComponent<PlayerStats>();

        Debug.Log(_playerStats);

        if (_playerStats != null)
        {
            _playerStats.TakeDamage(100);
        }
    }
}
