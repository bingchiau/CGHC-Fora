using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Respawn : MonoBehaviour
{
    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("References")]
    [SerializeField] private MooseBossAI mooseBossAI;
    [SerializeField] private MooseTailController mooseTailController;

    [Header("Scene Settings")]
    public string sceneToLoad;
    public float sceneLoadDelay = 2f; 
    private bool isLoadingScene = false;

    private GameObject currentPlayer;
    private PlayerStats currentStats;
    private Vector3 spawnPoint;
    private Camera2D mainCamera;

    private void Start()
    {
        spawnPoint = transform.position;
        mainCamera = FindObjectOfType<Camera2D>();
        SpawnPlayer();
    }

    private void Update()
    {
        if (currentPlayer == null || currentStats == null || isLoadingScene) return;

        if (currentStats.currentHealth <= 0)
        {
            StartCoroutine(LoadSceneAfterDelay());
        }
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

        if (mooseBossAI != null)
            mooseBossAI.player = currentPlayer.transform;

        if (mooseTailController != null)
            mooseTailController.player = currentPlayer.transform;

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

    private IEnumerator LoadSceneAfterDelay()
    {
        isLoadingScene = true;
        yield return new WaitForSeconds(sceneLoadDelay);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name not set in Level3Respawn script.");
        }
    }

}
