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

    [Header("Activation Sound")]
    [SerializeField] private AudioClip activationSound;
    [Range(0f, 1f)]
    [SerializeField] private float activationSoundVolume = 1f;
    [SerializeField] private float soundDuration = 2f;

    private AudioSource _audioSource;

    private void Awake()
    {
        // Create internal AudioSource
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.spatialBlend = 0f; // 0 = 2D sound
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(ActivationSequence());
    }

    private IEnumerator ActivationSequence()
    {
        // 🔊 Play activation sound
        if (activationSound != null)
        {
            _audioSource.volume = activationSoundVolume;
            _audioSource.clip = activationSound;
            _audioSource.Play();
        }

        // Fade in
        if (objectToFadeIn != null)
        {
            objectToFadeIn.SetActive(true);
            if (objectToFadeIn.TryGetComponent(out FadeEffect fadeIn))
                fadeIn.FadeIn(fadeDuration);
        }

        // Fade out
        if (objectToFadeOut != null)
        {
            if (objectToFadeOut.TryGetComponent(out FadeEffect fadeOut))
                fadeOut.FadeOut(fadeDuration);
        }

        // 🔊 Start BGM
        if (BackgroundMusicPlayer.Instance != null)
        {
            BackgroundMusicPlayer.Instance.PlayMusic();
        }
        else
        {
            Debug.LogWarning("[BossArenaActivator] BackgroundMusicPlayer.Instance not found.");
        }

        yield return new WaitForSeconds(soundDuration);

        boss?.SetActive(true);
        Debug.Log("[BossArenaActivator] Boss activated after delay.");
        gameObject.SetActive(false);
    }
}
