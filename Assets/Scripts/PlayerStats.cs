using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Здоровье")]
    public int MaxHP = 100;
    public int MinHP = 0;
    public int CurrentHP = 100;


    [Header("деньги")]
    [SerializeField]public float StealMoney;
    [SerializeField]public float PlayerMoney;
    public TextMeshProUGUI stealMoney;
    public TextMeshProUGUI myMoney;

    [Header("улучшения")]
    public float DealerTrusts;


    public bool IsAlive => CurrentHP > MinHP;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DealerTrusts = 0.7f;
    }

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, MinHP, MaxHP);
        Debug.Log($"Урон: -{amount} HP");

        if (CurrentHP <= MinHP)
            Die();
    }

    public void Heal(int amount)
    {
        if (!IsAlive)
        {
            Debug.Log("Лечение невозможно: персонаж мёртв.");
            return;
        }

        int before = CurrentHP;
        CurrentHP = Mathf.Clamp(CurrentHP + amount, MinHP, MaxHP);
        Debug.Log($"Лечение: +{CurrentHP - before} HP");
    }

    private void Die()
    {
        Debug.Log("Персонаж погиб.");
        Destroy(gameObject);
    }

    public void ChangeStealMoney(float money)
    {
        StealMoney += money;
        UpdateUI();
    }

    public void UpdateUI()
    {
        stealMoney.text = ($"Вкрадені гроші {StealMoney}");
        myMoney.text = ($"Мої гроші {PlayerMoney}");

    }
}