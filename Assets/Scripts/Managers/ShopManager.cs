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

        if (itemListContentParent == null) Debug.LogError("ShopManager: itemListContentParent не призначено!");
        if (shopItemUIPrefab == null) Debug.LogError("ShopManager: shopItemUIPrefab не призначено!");
        if (selectedItemPriceText == null) Debug.LogError("ShopManager: selectedItemPriceText не призначено!");
        if (buyButtonDetails == null) Debug.LogError("ShopManager: buyButtonDetails не призначено!");

        if (buyButtonDetails != null)
        {
            buyButtonDetails.onClick.RemoveAllListeners();
            buyButtonDetails.onClick.AddListener(TryBuyCurrentItem);
        }

        if (allShopItems.Count == 0)
        {
            LoadAllItems();
        }
    }

    void LoadAllItems()
    {
        if (allShopItems.Count == 0)
        {
            Debug.LogWarning("ShopManager: Список allShopItems порожній. Магазин не буде відображати предмети.");
        }
    }

    public void GenerateShopItemsUI()
    {
        foreach (Transform child in itemListContentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in allShopItems)
        {
            if (item == null) continue;

            GameObject uiItem = Instantiate(shopItemUIPrefab, itemListContentParent);
            ItemButton itemButton = uiItem.GetComponent<ItemButton>();

            if (itemButton != null)
            {
                itemButton.Initialize(item);
            }
            else
            {
                Debug.LogError($"ShopManager: Prefab '{shopItemUIPrefab.name}' не містить компонента ItemButton.");
            }
        }

        ItemButton firstItem = itemListContentParent.GetComponentInChildren<ItemButton>();
        if (firstItem != null)
        {
            SelectItemForDetails(firstItem);
        }
        else
        {
            UpdateItemDetailsUI(null);
        }
    }

    public void SelectItemForDetails(ItemButton itemButton)
    {
        currentSelectedItemButton = itemButton;
        UpdateItemDetailsUI(itemButton);
    }

    public void UpdateItemDetailsUI(ItemButton itemButton)
    {
        if (itemButton != null && itemButton.itemData != null)
        {
            selectedItemNameText.text = itemButton.itemData.itemName;
            selectedItemImage.sprite = itemButton.itemData.itemIcon;
            selectedItemImage.enabled = itemButton.itemData.itemIcon != null;
            selectedItemInfoText.text = itemButton.itemData.itemDescription;
            selectedItemPriceText.text = $"{itemButton.itemData.itemPrice}$";
            buyButtonDetails.interactable = true;

            foreach (Transform child in itemListContentParent)
            {
                child.GetComponent<Image>().color = (child.GetComponent<ItemButton>() == itemButton) ? Color.yellow : Color.white;
            }
        }
        else
        {
            selectedItemNameText.text = "Виберіть предмет";
            selectedItemImage.enabled = false;
            selectedItemInfoText.text = "Інформація про предмет";
            selectedItemPriceText.text = "---$";
            buyButtonDetails.interactable = false;
            currentSelectedItemButton = null;

            foreach (Transform child in itemListContentParent)
            {
                child.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void TryBuyCurrentItem()
    {
        if (currentSelectedItemButton != null && currentSelectedItemButton.itemData != null)
        {
            if (GameManager.Instance != null && PlayerStats.Instance != null && currentSelectedItemButton.itemData.spawnablePrefab != null)
            {
                if (GameManager.Instance.TryPurchaseItem(currentSelectedItemButton.itemData.itemPrice, currentSelectedItemButton.itemData.spawnablePrefab))
                {
                    Debug.Log($"Куплено {currentSelectedItemButton.itemData.itemName} за {currentSelectedItemButton.itemData.itemPrice}$!");
                    UpdateItemDetailsUI(currentSelectedItemButton);
                    PlayerStats.Instance.UpdateUI();
                }
                else
                {
                    Debug.LogWarning($"Недостатньо грошей для покупки {currentSelectedItemButton.itemData.itemName}.");
                }
            }
            else
            {
                Debug.LogError("ShopManager: PlayerStats.Instance або spawnablePrefab від вибраного предмета null. Неможливо здійснити покупку.");
            }
        }
        else
        {
            Debug.LogWarning("ShopManager: Немає вибраного предмета для покупки з панелі деталей.");
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            Debug.Log("Магазин відкрито.");
            PlayerStats.Instance?.UpdateUI();
            GenerateShopItemsUI();
            UpdateItemDetailsUI(null);
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Debug.Log("Магазин закрито.");
        }
    }
}