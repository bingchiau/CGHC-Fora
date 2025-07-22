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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(ActivationSequence());
    }

    private IEnumerator ActivationSequence()
    {
        // ⬛ Fade visuals
        if (objectToFadeIn != null)
        {
            objectToFadeIn.SetActive(true);
            if (objectToFadeIn.TryGetComponent(out FadeEffect fadeIn))
                fadeIn.FadeIn(fadeDuration);
        }

        if (objectToFadeOut != null)
        {
            if (objectToFadeOut.TryGetComponent(out FadeEffect fadeOut))
                fadeOut.FadeOut(fadeDuration);
        }

        // 🔁 Transition music using cinematic stinger
        if (BackgroundMusicPlayer.Instance != null)
        {
            BackgroundMusicPlayer.Instance.TransitionWithStinger(bossStinger, bossBGM, stingerDelay);
        }
        else
        {
            Debug.LogWarning("[BossArenaActivator] No BackgroundMusicPlayer found.");
        }

        yield return new WaitForSeconds(soundDuration);

        // ✅ Activate boss
        boss?.SetActive(true);
        Debug.Log("[BossArenaActivator] Boss activated.");

        gameObject.SetActive(false); // deactivate trigger
    }
}
