using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Economy")]
    public int playerMoney = 1000;

    [Header("Item Spawning")]
    public Transform playerTransform;
    public float spawnDistance = 2f;
    public LayerMask groundLayer;

    [Header("UI References")]
    public TextMeshProUGUI playerMoneyText;

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

    void Start()
    {
        UpdateMoneyUI();
    }

    public bool TryPurchaseItem(int price, GameObject itemToSpawnPrefab)
    {
        if (itemToSpawnPrefab == null)
        {
            Debug.LogError("GameManager: itemToSpawnPrefab is null. Cannot purchase.");
            return false;
        }

        if (playerMoney >= price)
        {
            playerMoney -= price;
            UpdateMoneyUI();
            Debug.Log($"������� ������� �� {price}$! ����������: {playerMoney}$");
            SpawnItem(itemToSpawnPrefab);
            return true;
        }
        else
        {
            Debug.LogWarning($"����������� ������! �������: {price}$, �: {playerMoney}$");
            return false;
        }
    }

    // ����� ��� ������ ��������
    public void SpawnItem(GameObject itemPrefab)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform �� ���������� � GameManager! ��������� �������� �������.");
            return;
        }

        Vector3 rayStartPoint = playerTransform.position + playerTransform.forward * spawnDistance;
        rayStartPoint.y += 2f;

        RaycastHit hit;
        Vector3 finalSpawnPosition = rayStartPoint;

        if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, 10f, groundLayer))
        {
            finalSpawnPosition = hit.point;
            finalSpawnPosition.y += 0.1f;
            Debug.Log($"������� ���� ���������� �� �������: {finalSpawnPosition}");
        }
        else
        {
            Debug.LogWarning($"GameManager: �� ������� ������ ����� ��� ������ �������� '{itemPrefab.name}' �� ��������� Raycast. ��������� � ��������� ������� ({rayStartPoint.y}Y). �������� groundLayer.");
        }

        Instantiate(itemPrefab, finalSpawnPosition, Quaternion.identity);
        Debug.Log($"������� '{itemPrefab.name}' ����������.");
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyUI();
        Debug.Log($"������ {amount}$! ������: {playerMoney}$");
    }

    public void UpdateMoneyUI()
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = $"{playerMoney}$";
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.UpdateItemDetailsUI(ShopManager.Instance.currentSelectedItemButton);
            }
        }
        else
        {
            Debug.LogWarning("Player Money Text (TextMeshProUGUI) �� ���������� � GameManager!");
        }
    }
}
