using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Level3BossActivator : MonoBehaviour
{
    [Header("Boss Reference")]
    [SerializeField] private GameObject boss;

    [Header("Music Transition")]
    [SerializeField] private AudioClip bossStinger;
    [SerializeField] private AudioClip bossBGM;
    [SerializeField] private float stingerDelay = 1.5f;

    [Header("Activation Delay")]
    [SerializeField] private float bossActivationDelay = 2f;

    private void Start()
    {
        // Prewarm the boss to avoid lag spike
        if (boss != null)
        {
            boss.SetActive(true);
            boss.SetActive(false);
        }

        // Preload audio clips
        bossStinger?.LoadAudioData();
        bossBGM?.LoadAudioData();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(ActivateBossSequence());
    }

    private IEnumerator ActivateBossSequence()
    {
        // Music transition
        if (BackgroundMusicPlayer.Instance != null)
        {
            BackgroundMusicPlayer.Instance.TransitionWithStinger(bossStinger, bossBGM, stingerDelay);
            Debug.Log("[Level3BossActivator] Music transition started.");
        }
        else
        {
            Debug.LogWarning("[Level3BossActivator] No BackgroundMusicPlayer found.");
        }

        // Wait before boss appears
        yield return WaitForUnscaledSeconds(bossActivationDelay);

        if (boss != null)
        {
            boss.SetActive(true);
            Debug.Log("[Level3BossActivator] Boss activated.");
        }

        gameObject.SetActive(false); // disable this trigger permanently
    }

    private IEnumerator WaitForUnscaledSeconds(float duration)
    {
        float endTime = Time.unscaledTime + duration;
        while (Time.unscaledTime < endTime)
            yield return null;
    }
}
