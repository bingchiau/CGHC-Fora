using UnityEngine;
using System.Collections;

public class BossDeathHandler : MonoBehaviour
{
    /// <summary>
    /// Call this to handle boss death: deactivate + destroy + show death scene
    /// </summary>
    public void HandleBossDeath(GameObject boss, GameObject bossDeathScene, float delay)
    {
        StartCoroutine(DeathSequence(boss, bossDeathScene, delay));
    }

    private IEnumerator DeathSequence(GameObject boss, GameObject bossDeathScene, float delay)
    {
        boss.SetActive(false);
        yield return new WaitForSeconds(delay);
        if (bossDeathScene != null) bossDeathScene.SetActive(true);
        Destroy(boss);
    }

    /// <summary>
    /// NEW: Call this to shake the camera after a delay — for dramatic effects AFTER death
    /// </summary>
    public void ShakeCameraAfterDelay(Camera2D camera2D, float delay, float duration, float magnitude)
    {
        StartCoroutine(ShakeAfterDelayCoroutine(camera2D, delay, duration, magnitude));
    }

    private IEnumerator ShakeAfterDelayCoroutine(Camera2D camera2D, float delay, float duration, float magnitude)
    {
        yield return new WaitForSeconds(delay);
        if (camera2D != null) camera2D.ShakeCamera(duration, magnitude);
    }
}
