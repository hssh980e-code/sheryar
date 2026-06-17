using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays player health on UI.
/// </summary>
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;

    private Health playerHealth;

    private void Start()
    {
        // Find player's health component
        playerHealth = FindObjectOfType<PlayerController>().GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.OnDamaged.AddListener(UpdateHealthUI);
            playerHealth.OnHealed.AddListener(UpdateHealthUI);
            UpdateHealthUI(0);
        }
    }

    private void UpdateHealthUI(float amount)
    {
        if (playerHealth == null) return;

        float healthPercent = playerHealth.GetHealthPercent();

        // Update health bar
        if (healthBar != null)
        {
            healthBar.fillAmount = healthPercent;

            // Color based on health
            healthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        }

        // Update health text
        if (healthText != null)
        {
            healthText.text = $"{playerHealth.GetHealth():F0} / {playerHealth.GetMaxHealth():F0}";
        }
    }
}
