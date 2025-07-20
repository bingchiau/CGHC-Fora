using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHealthEscapeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossToEscape;
    [SerializeField] private GameObject bossEscapeScene;
    [SerializeField] private float escapeDelay = 2f;
    [SerializeField] private int triggerHealthThreshold = 40;

    private bool hasTriggered = false;

    private void Update()
    {
        if (hasTriggered || PlayerStats.Instance == null || BossEscapeHandler.Instance == null)
            return;

        if (PlayerStats.Instance.CurrentHealth <= triggerHealthThreshold)
        {
            hasTriggered = true;
            Debug.Log("[PlayerHealthEscapeTrigger] Player HP dropped to threshold. Triggering boss escape...");

            BossEscapeHandler.Instance.HandleBossEscape(
                bossToEscape,
                bossEscapeScene,
                escapeDelay
            );
        }
    }
}
