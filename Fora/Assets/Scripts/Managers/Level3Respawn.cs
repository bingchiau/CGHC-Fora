using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Respawn : MonoBehaviour
{
    [Header("Player Prefab")]
    public GameObject playerPrefab;

    private GameObject currentPlayer;
    private PlayerStats currentStats;
    private Vector3 spawnPoint;

    private Camera2D mainCamera;

    [SerializeField] private float respawnDelay = 2f; // Delay in seconds
    private bool isRespawning = false; // Prevent multiple coroutines

    private void Start()
    {
        spawnPoint = transform.position;
        mainCamera = FindObjectOfType<Camera2D>();

        SpawnPlayer();
    }

    private void Update()
    {
        if (currentPlayer == null || currentStats == null || isRespawning) return;

        if (currentStats.currentHealth <= 0)
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        isRespawning = true;

        Destroy(currentPlayer);
        currentStats = null;

        yield return new WaitForSeconds(respawnDelay);

        SpawnPlayer();
        isRespawning = false;
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        currentStats = currentPlayer.GetComponent<PlayerStats>();

        if (currentStats == null)
        {
            Debug.LogError("Spawned player is missing PlayerStats.");
            return;
        }

        currentStats.currentHealth = currentStats.maxHealth;

        if (mainCamera != null)
        {
            if (!mainCamera.enabled) mainCamera.enabled = true;

            PlayerMotor motor = currentPlayer.GetComponent<PlayerMotor>();
            if (motor != null)
            {
                mainCamera.SetTarget(motor);
            }
        }
    }
}
