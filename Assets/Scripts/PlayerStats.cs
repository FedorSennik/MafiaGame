using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float minHealth = 0f;
    public float currentHealth;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);
        Debug.Log("Урон: -" + amount + " HP");

        if (currentHealth <= minHealth)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);
        Debug.Log("Лечение: +" + amount + " HP");
    }

    void Die()
    {
        Destroy(gameObject);
    }
}