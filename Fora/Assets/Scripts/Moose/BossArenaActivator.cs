using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BossArenaActivator : MonoBehaviour
{
    [Header("Boss & Visuals")]
    [SerializeField] private GameObject boss;

    [Header("Fade Targets")]
    [SerializeField] private GameObject objectToFadeIn;
    [SerializeField] private GameObject objectToFadeOut;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Music Transition")]
    [SerializeField] private AudioClip bossStinger;
    [SerializeField] private AudioClip bossBGM;
    [SerializeField] private float stingerDelay = 1.5f;

    [Header("Delay Before Boss Appears")]
    [SerializeField] private float soundDuration = 2f;

    private void Start()
    {
        // Prewarm the boss to avoid lag spike on activation
        if (boss != null)
        {
            boss.SetActive(true);
            boss.SetActive(false);
        }

        // Preload audio if needed (optional safety for large clips)
        if (bossStinger != null)
            bossStinger.LoadAudioData();
        if (bossBGM != null)
            bossBGM.LoadAudioData();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(ActivationSequence());
    }

    private IEnumerator ActivationSequence()
    {
        // Frame 1: Set fade object active
        if (objectToFadeIn != null)
        {
            objectToFadeIn.SetActive(true);
            yield return null;
        }

        // Frame 2: Trigger fade in
        if (objectToFadeIn.TryGetComponent(out FadeEffect fadeIn))
            fadeIn.FadeIn(fadeDuration);
        yield return null;

        // Frame 3: Trigger fade out
        if (objectToFadeOut != null && objectToFadeOut.TryGetComponent(out FadeEffect fadeOut))
            fadeOut.FadeOut(fadeDuration);
        yield return null;

        // Frame 4: Wait a moment before stinger transition
        yield return WaitForUnscaledSeconds(1f);

        // Music transition
        if (BackgroundMusicPlayer.Instance != null)
        {
            BackgroundMusicPlayer.Instance.TransitionWithStinger(bossStinger, bossBGM, stingerDelay);
        }
        else
        {
            Debug.LogWarning("[BossArenaActivator] No BackgroundMusicPlayer found.");
        }

        yield return WaitForUnscaledSeconds(soundDuration);

        // Frame 6+: Activate boss
        boss?.SetActive(true);
        Debug.Log("[BossArenaActivator] Boss activated.");

        gameObject.SetActive(false); // disable trigger to prevent reuse
    }

    private IEnumerator WaitForUnscaledSeconds(float time)
    {
        float start = Time.unscaledTime;
        while (Time.unscaledTime < start + time)
            yield return null;
    }
}
