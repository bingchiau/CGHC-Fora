using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private LevelManager _levelManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("Player"))
        {  
            _levelManager.SetNewSpawnPoint(_spawnPoint);
        }
    }
}
