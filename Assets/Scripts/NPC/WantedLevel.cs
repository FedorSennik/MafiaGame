using UnityEngine;

public class WantedLevel : MonoBehaviour
{
    public static WantedLevel Instance { get; private set; }

    [Header("������������ ���� �������")]
    public int wantedLevel = 0;
    public float searchTime = 30f;

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
                Debug.Log("г���� ������� �������. ������ �� ��������.");
            }
        }
    }

    public void IncreaseWantedLevel(int amount)
    {
        wantedLevel += amount;
        searchTimer = searchTime;
        isBeingSearched = true;
        Debug.Log($"г���� ������� �������� �� {amount}. �������� �����: {wantedLevel}");
    }
}