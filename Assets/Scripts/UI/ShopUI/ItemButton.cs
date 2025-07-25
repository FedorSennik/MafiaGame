using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public Item itemData;

    [Header("UI References (Assign in Prefab Inspector)")]
    public TextMeshProUGUI itemNameText;
    public Image itemIconImage;
    public Button selectButton;

    void Awake()
    {
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnSelectButtonClicked);
        }
        else
        {
            Debug.LogError($"ItemButton: SelectButton not assigned on {gameObject.name}.");
        }
    }

    public void Initialize(Item item)
    {
        itemData = item;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (itemData == null)
        {
            Debug.LogWarning($"ItemButton: itemData is null on {gameObject.name}. Cannot update UI.");
            return;
        }

        if (itemNameText != null)
        {
            itemNameText.text = itemData.itemName;
        }
        else
        {
            Debug.LogError($"ItemButton: itemNameText (TextMeshProUGUI) not assigned on {gameObject.name}.");
        }

        if (itemIconImage != null)
        {
            if (itemData.itemIcon != null)
            {
                itemIconImage.sprite = itemData.itemIcon;
                itemIconImage.enabled = true;
            }
            else
            {
                itemIconImage.sprite = null;
                itemIconImage.enabled = false;
                Debug.LogWarning($"ItemButton: itemIconImage sprite is null for '{itemData.itemName}' on {gameObject.name}. Hiding image.");
            }
        }
        else
        {
            Debug.LogError($"ItemButton: itemIconImage (Image) not assigned on {gameObject.name}.");
        }
    }

    public void OnSelectButtonClicked()
    {
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.SelectItemForDetails(this);
            Debug.Log($"Вибрано {itemData.itemName} для деталей.");
        }
        else
        {
            Debug.LogError("ItemButton: ShopManager.Instance is null. Cannot select item for details.");
        }
    }
}
