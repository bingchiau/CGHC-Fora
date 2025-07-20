using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossArenaActivator : MonoBehaviour
{
    [Header("Boss & Visual")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject objectToFadeIn;
    [SerializeField] private float fadeDuration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        boss?.SetActive(true);

        if (objectToFadeIn != null)
        {
            objectToFadeIn.SetActive(true);

            if (objectToFadeIn.TryGetComponent(out FadeEffect fade))
                fade.FadeIn(fadeDuration);
        }

        Debug.Log("[BossArenaActivator] Boss activated, object fading in.");
        gameObject.SetActive(false);
    }
}
