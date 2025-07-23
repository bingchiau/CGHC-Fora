using UnityEngine;
using System.Collections;

public class DashRefill : MonoBehaviour
{
    public GameObject playerObject;
    public float cooldownTime = 5f;
    public float fadeDuration = 1f;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool isOnCooldown = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (spriteRenderer == null)
            Debug.LogWarning("DashRefill: No SpriteRenderer found!");

        if (playerObject == null)
        {
            StartCoroutine(FindPlayerObjectWithDelay());
        }
    }

    private IEnumerator FindPlayerObjectWithDelay()
    {
        float timer = 0f;
        while (playerObject == null && timer < 5f)
        {
            PlayerDash playerDash = FindObjectOfType<PlayerDash>();
            if (playerDash != null)
            {
                playerObject = playerDash.gameObject;
                Debug.Log("DashRefill: Found PlayerDash.");
                yield break;
            }

            timer += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        if (playerObject == null)
            Debug.LogWarning("DashRefill: No PlayerDash found in scene after waiting.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOnCooldown || playerObject == null) return;

        if (other.gameObject == playerObject)
        {
            PlayerDash playerDash = playerObject.GetComponent<PlayerDash>();
            if (playerDash != null)
            {
                playerDash.RefillDash();
                StartCoroutine(RefillCooldown());
            }
        }
    }

    private IEnumerator RefillCooldown()
    {
        isOnCooldown = true;
        col.enabled = false;
        SetAlpha(0f); // Instantly hide

        yield return new WaitForSeconds(cooldownTime);

        // Fade back in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            SetAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
        col.enabled = true;
        isOnCooldown = false;
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
