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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(ActivationSequence());
    }

    private System.Collections.IEnumerator ActivationSequence()
    {
        // Fade in
        if (objectToFadeIn != null)
        {
            objectToFadeIn.SetActive(true); // Ensure it's active to fade in
            if (objectToFadeIn.TryGetComponent(out FadeEffect fadeIn))
                fadeIn.FadeIn(fadeDuration);
        }

        // Fade out
        if (objectToFadeOut != null)
        {
            if (objectToFadeOut.TryGetComponent(out FadeEffect fadeOut))
                fadeOut.FadeOut(fadeDuration);
        }

        // Wait before activating boss
        yield return new WaitForSeconds(3f);

        boss?.SetActive(true);

        Debug.Log("[BossArenaActivator] Boss activated after delay.");
        gameObject.SetActive(false);
    }
}
