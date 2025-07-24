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
            Debug.LogError("Health Text (TextMeshProUGUI) �� ���������� � HealthBarUI ������!");
            return;
        }

        if (PlayerStats.Instance == null)
        {
            Debug.LogWarning("PlayerStats.Instance �� ��������. UI ������'� �� ���� ������������.");
            healthText.text = "N/A";
            healthText.color = Color.grey;
            return;
        }

        float currentHealth = PlayerStats.Instance.currentHealth;
        float maxHealth = PlayerStats.Instance.maxHealth;

        healthText.text = Mathf.RoundToInt(currentHealth).ToString();

        // ������'������: ���� ������� ������ ������� �� ������'�
        float healthPercentage = currentHealth / maxHealth;
        if (healthPercentage < 0.25f) // ���� ������'� ����� 25%
        {
            healthText.color = Color.red;
        }
        else if (healthPercentage < 0.5f) // ���� ������'� ����� 50%
        {
            healthText.color = Color.yellow;
        }
        else // ������ (������'� ����� 50%)
        {
            healthText.color = Color.white; // ��� Color.green
        }
    }
}