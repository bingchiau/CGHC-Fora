using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BossEscapeHandler : MonoBehaviour
{
    /// <summary>
    /// Handles boss escape: deactivate, wait, activate escape scene, and destroy boss.
    /// </summary>
    public void HandleBossEscape(GameObject boss, GameObject bossEscapeScene, float delay)
    {
        StartCoroutine(EscapeSequence(boss, bossEscapeScene, delay));
    }

    private IEnumerator EscapeSequence(GameObject boss, GameObject bossEscapeScene, float delay)
    {
        boss.SetActive(false);
        yield return new WaitForSeconds(delay);

        if (bossEscapeScene != null)
            bossEscapeScene.SetActive(true);

        Destroy(boss);
    }

    /// <summary>
    /// Triggers a delayed camera shake — useful for dramatic escape moments.
    /// </summary>
    public void ShakeCameraAfterDelay(Camera2D camera, float delay, float duration, float magnitude)
    {
        StartCoroutine(ShakeAfterDelay(camera, delay, duration, magnitude));
    }

    private IEnumerator ShakeAfterDelay(Camera2D camera, float delay, float duration, float magnitude)
    {
        yield return new WaitForSeconds(delay);
        camera?.ShakeCamera(duration, magnitude);
    }
}
