using UnityEngine;
using TMPro;

public class WantedLevel : MonoBehaviour
{
    public static WantedLevel Instance { get; private set; }

    [Header("Налаштування рівня розшуку")]
    public int wantedLevel = 0;
    public float searchTime = 30f;

    [Header("UI Елементи")]
    public TextMeshProUGUI wantedLevelText;

    [Header("Статус гравця")]
    public bool isPlayerInside = false;

    private float searchTimer = 0f;
    private bool isBeingSearched = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (wantedLevel > 0 && !isBeingSearched)
        {
            isBeingSearched = true;
            searchTimer = searchTime;
        }

        if (isBeingSearched)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0)
            {
                isBeingSearched = false;
                wantedLevel = 0;
                Debug.Log("Рівень розшуку скинуто. Гравця не знайдено.");
            }
        }

        UpdateUI();
    }

    public void IncreaseWantedLevel(int amount)
    {
        wantedLevel += amount;
        searchTimer = searchTime;
        isBeingSearched = true;
        Debug.Log($"Рівень розшуку підвищено на {amount}. Поточний рівень: {wantedLevel}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (wantedLevelText != null)
        {
            if (wantedLevel > 0)
            {
                wantedLevelText.text = $"Рівень розшуку: {wantedLevel}";
            }
            else
            {
                wantedLevelText.text = "";
            }
        }
    }
}