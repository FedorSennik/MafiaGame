using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public float maxHealth = 100f;
    // Поточне значення здоров'я (для тестування)
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    void Update()
    {
        // --- Для тестування: Зміна здоров'я за допомогою клавіш ---
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10f); // Віднімаємо 10 здоров'я
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Heal(10f); // Додаємо 10 здоров'я
        }
        // --- Кінець тестової логіки ---
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthText();
    }

    // Приклад функції для отримання шкоди
    public void TakeDamage(float amount)
    {
        SetHealth(currentHealth - amount);
        Debug.Log($"Отримано {amount} шкоди. Поточне здоров'я: {currentHealth}");
    }

    // Приклад функції для лікування
    public void Heal(float amount)
    {
        SetHealth(currentHealth + amount);
        Debug.Log($"Відновлено {amount} здоров'я. Поточне здоров'я: {currentHealth}");
    }

    void UpdateHealthText()
    {
        if (healthText == null)
        {
            Debug.LogError("Health Text (TextMeshProUGUI) не призначено в HealthBarUI скрипті!");
            return;
        }

        healthText.text = Mathf.RoundToInt(currentHealth).ToString();

        // --- Необов'язково: Зміна кольору тексту залежно від здоров'я ---
        float healthPercentage = currentHealth / maxHealth;
        if (healthPercentage < 0.25f) // Якщо здоров'я менше 25%
        {
            healthText.color = Color.red;
        }
        else if (healthPercentage < 0.5f) // Якщо здоров'я менше 50%
        {
            healthText.color = Color.yellow;
        }
        else // Інакше (здоров'я більше 50%)
        {
            healthText.color = Color.white; // Або Color.green, якщо хочете
        }
    }
}