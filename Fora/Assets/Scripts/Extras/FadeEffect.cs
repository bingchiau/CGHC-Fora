using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tilemapRenderer;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    public void FadeIn(float duration)
    {
        StartFade(0f, 1f, duration);
    }

    public void FadeOut(float duration)
    {
        StartFade(GetAlpha(), 0f, duration);
    }

    private void StartFade(float fromAlpha, float toAlpha, float duration)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(fromAlpha, toAlpha, duration));
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        Color startColor = GetColor();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, t);

            Color newColor = startColor;
            newColor.a = alpha;
            SetColor(newColor);
            yield return null;
        }

        Color finalColor = startColor;
        finalColor.a = toAlpha;
        SetColor(finalColor);
    }

    private float GetAlpha()
    {
        return GetColor().a;
    }

    private Color GetColor()
    {
        if (spriteRenderer != null) return spriteRenderer.color;
        if (tilemapRenderer != null) return tilemapRenderer.material.color;
        return Color.white;
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;

        if (tilemapRenderer != null)
            tilemapRenderer.material.color = color;
    }
}
