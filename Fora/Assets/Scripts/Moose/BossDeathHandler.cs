using UnityEngine;
using System.Collections;

public class BossDeathHandler : MonoBehaviour
{
    /// <summary>
    /// Call this to handle boss death in a robust way.
    /// </summary>
    /// <param name="boss">The boss GameObject to deactivate and destroy</param>
    /// <param name="bossDeathScene">The GameObject to activate</param>
    /// <param name="delay">How long to wait before activating and destroying</param>
    public void HandleBossDeath(GameObject boss, GameObject bossDeathScene, float delay)
    {
        StartCoroutine(DeathSequence(boss, bossDeathScene, delay));
    }

    private IEnumerator DeathSequence(GameObject boss, GameObject bossDeathScene, float delay)
    {
        // ✅ Fully deactivate the boss GameObject
        boss.SetActive(false);

        // ✅ Wait for the delay
        yield return new WaitForSeconds(delay);

        // ✅ Activate the death scene if assigned
        if (bossDeathScene != null)
        {
            bossDeathScene.SetActive(true);
        }

        // ✅ Destroy the boss GameObject to clean up
        Destroy(boss);
    }
}
