using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void Start()
    {
        UpdateHealthText();
    }

    void Update()
    {
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (healthText == null)
        {
            Debug.LogError("Health Text (TextMeshProUGUI) не призначено в HealthBarUI скрипт≥!");
            return;
        }

        if (PlayerStats.Instance == null)
        {
            Debug.LogWarning("PlayerStats.Instance не знайдено. UI здоров'€ не буде оновлюватис€.");
            healthText.text = "N/A";
            healthText.color = Color.grey;
            return;
        }

        float currentHealth = PlayerStats.Instance.currentHealth;
        float maxHealth = PlayerStats.Instance.maxHealth;

        healthText.text = Mathf.RoundToInt(currentHealth).ToString();

        // Ќеобов'€зково: «м≥на кольору тексту залежно в≥д здоров'€
        float healthPercentage = currentHealth / maxHealth;
        if (healthPercentage < 0.25f) // якщо здоров'€ менше 25%
        {
            healthText.color = Color.red;
        }
        else if (healthPercentage < 0.5f) // якщо здоров'€ менше 50%
        {
            healthText.color = Color.yellow;
        }
        else // ≤накше (здоров'€ б≥льше 50%)
        {
            healthText.color = Color.white; // јбо Color.green
        }
    }
}