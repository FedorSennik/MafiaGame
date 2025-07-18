using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public float maxHealth = 100f;
    // ������� �������� ������'� (��� ����������)
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    void Update()
    {
        // --- ��� ����������: ���� ������'� �� ��������� ����� ---
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10f); // ³������ 10 ������'�
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Heal(10f); // ������ 10 ������'�
        }
        // --- ʳ���� ������� ����� ---
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthText();
    }

    // ������� ������� ��� ��������� �����
    public void TakeDamage(float amount)
    {
        SetHealth(currentHealth - amount);
        Debug.Log($"�������� {amount} �����. ������� ������'�: {currentHealth}");
    }

    // ������� ������� ��� ��������
    public void Heal(float amount)
    {
        SetHealth(currentHealth + amount);
        Debug.Log($"³�������� {amount} ������'�. ������� ������'�: {currentHealth}");
    }

    void UpdateHealthText()
    {
        if (healthText == null)
        {
            Debug.LogError("Health Text (TextMeshProUGUI) �� ���������� � HealthBarUI ������!");
            return;
        }

        healthText.text = Mathf.RoundToInt(currentHealth).ToString();

        // --- ������'������: ���� ������� ������ ������� �� ������'� ---
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
            healthText.color = Color.white; // ��� Color.green, ���� ������
        }
    }
}