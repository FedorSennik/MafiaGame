using UnityEngine;

public class NPCStats : MonoBehaviour
{
    public int MaxHP = 100;
    public int CurrentHP;

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        Debug.Log($"NPC отримав урон: -{amount} HP. Залишилось: {CurrentHP} HP.");

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("NPC знищено.");

        Destroy(gameObject);
    }
}