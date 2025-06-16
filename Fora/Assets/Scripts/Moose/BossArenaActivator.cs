using UnityEngine;

public class BossArenaActivator : MonoBehaviour
{
    [Header("The boss GameObject to activate")]
    public GameObject boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (boss != null)
            {
                boss.SetActive(true);
                Debug.Log("Boss Activated!");
            }

            // Optional: disable this trigger so it doesn't fire again
            gameObject.SetActive(false);
        }
    }
}
