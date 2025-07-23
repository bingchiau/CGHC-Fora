using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BossDeathHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip deathSFX;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    private AudioSource _audioSource;

    [Header("Music Control")]
    [SerializeField] private bool stopBGMOnDeath = true;

    [Header("Reward Drop")]
    [SerializeField] private GameObject itemToSpawn;

    [Tooltip("Optional: If set, item will be spawned at this point instead of boss position.")]
    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f; // 2D sound
    }

    public void HandleBossDeath()
    {
        Debug.Log("[BossDeathHandler] Handling boss death...");

        // Stop background music
        if (stopBGMOnDeath && BackgroundMusicPlayer.Instance != null)
        {
            BackgroundMusicPlayer.Instance.StopMusic();
        }

        // Play reliable one-shot SFX
        if (deathSFX != null)
        {
            _audioSource.PlayOneShot(deathSFX, sfxVolume);
        }

        // Spawn reward at specified point or at boss position
        if (itemToSpawn != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
        }
    }
}
