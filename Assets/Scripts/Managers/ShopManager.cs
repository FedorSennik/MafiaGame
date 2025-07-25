using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Shop UI Panels")]
    public GameObject shopPanel;
    public Transform itemListContentParent;
    public GameObject shopItemUIPrefab;

    [Header("Item Details Panel")]
    public TextMeshProUGUI selectedItemNameText;
    public Image selectedItemImage;
    public TextMeshProUGUI selectedItemInfoText;
    public TextMeshProUGUI selectedItemPriceText;
    public Button buyButtonDetails;

    [Header("Categories")]
    public Transform categoryButtonsParent;
    public GameObject categoryButtonPrefab;

    public List<Item> allShopItems = new List<Item>();

    public ItemButton currentSelectedItemButton;

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
        if (shopPanel != null) shopPanel.SetActive(false);

        if (itemListContentParent == null) Debug.LogError("ShopManager: itemListContentParent �� ����������!");
        if (shopItemUIPrefab == null) Debug.LogError("ShopManager: shopItemUIPrefab �� ����������!");
        if (selectedItemPriceText == null) Debug.LogError("ShopManager: selectedItemPriceText �� ����������!");
        if (buyButtonDetails == null) Debug.LogError("ShopManager: buyButtonDetails �� ����������!");

        if (allShopItems == null || allShopItems.Count == 0)
        {
            Debug.LogWarning("ShopManager: ������ 'All Shop Items' (Item ScriptableObjects) �������. ������� �� ���� ���������� ��������.");
        }

        GenerateShopItemsUI(null);
        SetupCategoryButtons();

        if (buyButtonDetails != null)
        {
            buyButtonDetails.onClick.RemoveAllListeners();
            buyButtonDetails.onClick.AddListener(OnBuyButtonDetailsClicked);
        }

        UpdateItemDetailsUI(null);
    }

    public void GenerateShopItemsUI(string categoryFilter)
    {
        Debug.Log($"ShopManager: �������� UI ��� ��������. Գ����: '{categoryFilter}'. ������ Item ScriptableObjects: {allShopItems.Count}");

        foreach (Transform child in itemListContentParent)
        {
            Destroy(child.gameObject);
        }

        List<Item> filteredItems = new List<Item>();

        if (string.IsNullOrEmpty(categoryFilter) || categoryFilter == "All")
        {
            filteredItems = allShopItems;
            Debug.Log($"ShopManager: Գ���� 'All' ��� �������. ������ �������� ��� �����������: {filteredItems.Count}");
        }
        else
        {
            filteredItems = allShopItems
                .Where(item => item != null && item.itemTag == categoryFilter)
                .ToList();
            Debug.Log($"ShopManager: Գ���� �� �������� '{categoryFilter}'. �������� ��������: {filteredItems.Count}");
        }

        if (filteredItems.Count == 0)
        {
            Debug.LogWarning("ShopManager: ���� �������� ��� ����������� ���� ����������.");
        }

        foreach (Item itemData in filteredItems)
        {
            if (itemData == null)
            {
                Debug.LogWarning("ShopManager: Item ScriptableObject � ������ 'allShopItems' � null. ���������.");
                continue;
            }
            if (itemData.spawnablePrefab == null)
            {
                Debug.LogWarning($"ShopManager: 'Spawnable Prefab' �� ���������� ��� Item ScriptableObject '{itemData.itemName}'. ��� ������� �� ���� ���� ��������.");
                continue;
            }

            GameObject itemUI = Instantiate(shopItemUIPrefab, itemListContentParent);
            ItemButton itemButton = itemUI.GetComponent<ItemButton>();
            if (itemButton != null)
            {
                itemButton.Initialize(itemData);
                Debug.Log($"ShopManager: �������� UI ������� ��� '{itemData.itemName}'.");
            }
            else
            {
                Debug.LogError($"ShopManager: Prefab '{shopItemUIPrefab.name}' �� �� ���������� ItemButton. UI ������� �� ���� �������������.");
            }
        }
        UpdateItemDetailsUI(null);
    }

    void SetupCategoryButtons()
    {
        if (categoryButtonsParent != null)
        {
            foreach (Transform child in categoryButtonsParent)
            {
                Button btn = child.GetComponent<Button>();
                TextMeshProUGUI btnText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (btn != null && btnText != null)
                {
                    string categoryName = btnText.text;
                    string trimmedCategoryName = categoryName.Trim();

                    btn.onClick.RemoveAllListeners();
                    if (trimmedCategoryName == "All")
                    {
                        btn.onClick.AddListener(() => GenerateShopItemsUI(null));
                    }
                    else
                    {
                        btn.onClick.AddListener(() => GenerateShopItemsUI(trimmedCategoryName));
                    }
                    Debug.Log($"ShopManager: ����������� ������ ������� '{trimmedCategoryName}'.");
                }
            }
        }
        else
        {
            Debug.LogWarning("ShopManager: categoryButtonsParent �� ����������. ������ �������� �� ������ ����������.");
        }
    }

    public void SelectItemForDetails(ItemButton itemButton)
    {
        currentSelectedItemButton = itemButton;
        UpdateItemDetailsUI(itemButton);
    }

    public void UpdateItemDetailsUI(ItemButton itemButton)
    {
        if (itemButton == null || itemButton.itemData == null)
        {
            if (selectedItemNameText != null) selectedItemNameText.text = "����� ��������";
            if (selectedItemImage != null)
            {
                selectedItemImage.sprite = null;
                selectedItemImage.enabled = false;
            }
            if (selectedItemInfoText != null) selectedItemInfoText.text = "���������� ��� �������...";
            if (selectedItemPriceText != null) selectedItemPriceText.text = "";
            if (buyButtonDetails != null) buyButtonDetails.interactable = false;
            return;
        }

        if (selectedItemNameText != null) selectedItemNameText.text = itemButton.itemData.itemName;
        if (selectedItemImage != null)
        {
            if (itemButton.itemData.itemIcon != null)
            {
                selectedItemImage.sprite = itemButton.itemData.itemIcon;
                selectedItemImage.enabled = true;
            }
            else
            {
                selectedItemImage.sprite = null;
                selectedItemImage.enabled = false;
                Debug.LogWarning($"ShopManager: Icon sprite is null for '{itemButton.itemData.itemName}'. Hiding image in details panel.");
            }
        }
        if (selectedItemInfoText != null) selectedItemInfoText.text = itemButton.itemData.itemDescription;

        if (selectedItemPriceText != null) selectedItemPriceText.text = itemButton.itemData.itemPrice.ToString() + "$";

        if (buyButtonDetails != null)
        {
            buyButtonDetails.interactable = GameManager.Instance != null && GameManager.Instance.playerMoney >= itemButton.itemData.itemPrice;
        }
    }

    public void OnBuyButtonDetailsClicked()
    {
        if (currentSelectedItemButton != null && currentSelectedItemButton.itemData != null)
        {
            if (GameManager.Instance != null && currentSelectedItemButton.itemData.spawnablePrefab != null)
            {
                if (GameManager.Instance.TryPurchaseItem(currentSelectedItemButton.itemData.itemPrice, currentSelectedItemButton.itemData.spawnablePrefab))
                {
                    Debug.Log($"������� {currentSelectedItemButton.itemData.itemName} �� {currentSelectedItemButton.itemData.itemPrice}$!");
                    UpdateItemDetailsUI(currentSelectedItemButton);
                }
                else
                {
                    Debug.LogWarning($"����������� ������ ��� ������� {currentSelectedItemButton.itemData.itemName}.");
                }
            }
            else
            {
                Debug.LogError("ShopManager: GameManager.Instance ��� spawnablePrefab �� ��������� �������� null. ��������� �������� �������.");
            }
        }
        else
        {
            Debug.LogWarning("ShopManager: ���� ��������� �������� ��� ������� � ����� �������.");
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            Debug.Log("������� �������.");
            GameManager.Instance?.UpdateMoneyUI();
            GenerateShopItemsUI(null);
            UpdateItemDetailsUI(null);
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Debug.Log("������� �������.");
        }
    }
}
