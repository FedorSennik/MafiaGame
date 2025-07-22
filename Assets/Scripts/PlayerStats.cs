using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float minHealth = 0f;
    public float currentHealth;

    [Header("Ammo Settings")]
    public float currentAmmo = 30f;
    public float maxAmmo = 120f;

    [Header("UI Elements")]
    public Text healthText;
    public Text ammoText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();  // Показываем стартовое состояние UI
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);

        Debug.Log("Урон: -" + amount + " HP");

        UpdateUI();

        if (currentHealth <= minHealth)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);

        Debug.Log("Лечение: +" + amount + " HP");

        UpdateUI();
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0f, maxAmmo);

        Debug.Log("Патроны добавлены: +" + amount);

        UpdateUI();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"HP: {currentHealth}";

        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmo} / {maxAmmo}";
    }
}