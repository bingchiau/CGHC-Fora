using UnityEngine;
using System.Collections;

public class BossDeathHandler : MonoBehaviour
{
    public void HandleBossDeath(GameObject boss, GameObject bossDeathScene, float delay)
    {
        StartCoroutine(DeathSequence(boss, bossDeathScene, delay));
    }

    private IEnumerator DeathSequence(GameObject boss, GameObject bossDeathScene, float delay)
    {
        boss.SetActive(false);
        yield return new WaitForSeconds(delay);

        if (bossDeathScene != null)
            bossDeathScene.SetActive(true);

        Destroy(boss);
    }

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