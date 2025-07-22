using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Optional Settings")]
    [SerializeField] private bool showMaxHealth = false;
    [SerializeField] private string format = "Health ({0})"; 


    private void Start()
    {
        if (PlayerStats.Instance != null)
        {
            UpdateHealthText(PlayerStats.Instance.CurrentHealth);
            PlayerStats.Instance.OnHealthChanged += UpdateHealthText;
        }
    }

    private void OnDestroy()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged -= UpdateHealthText;
        }
    }

    private void UpdateHealthText(int currentHealth)
    {
        if (healthText == null)
        {
            Debug.LogWarning("HealthText UI is not assigned.");
            return;
        }

        Debug.Log($"[Health UI] Updating health text: {currentHealth}");

        if (showMaxHealth)
        {
            int maxHealth = PlayerStats.Instance.maxHealth;
            healthText.text = string.Format(format, currentHealth, maxHealth);
        }
        else
        {
            healthText.text = string.Format(format, currentHealth);
        }
    }

}
