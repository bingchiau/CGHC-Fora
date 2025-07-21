using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossArenaActivator : MonoBehaviour
{
    [Header("Boss & Visuals")]
    [SerializeField] private GameObject boss;

    [Header("Fade Targets")]
    [SerializeField] private GameObject objectToFadeIn;
    [SerializeField] private GameObject objectToFadeOut;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip activationSound;
    [SerializeField] private float soundDuration = 2f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(ActivationSequence());
    }

    private System.Collections.IEnumerator ActivationSequence()
    {
        // Play sound
        if (activationSound != null)
        {
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

        // Wait for the sound duration
        yield return new WaitForSeconds(soundDuration);

        boss?.SetActive(true);

        Debug.Log("[BossArenaActivator] Boss activated after delay.");
        gameObject.SetActive(false);
    }
}
