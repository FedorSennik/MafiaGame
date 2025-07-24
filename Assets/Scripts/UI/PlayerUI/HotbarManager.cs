using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HotbarManager : MonoBehaviour
{
    public Image[] hotbarSlots;
    public Item[] hotbarItems;

    public int selectedSlot = 0;

    // Список дозволених тегів для предметів, які можуть потрапити в Hotbar
    // Заповнюється в Inspector.
    public List<string> allowedHotbarTags = new List<string>();

    void Start()
    {
        hotbarItems = new Item[5];
        UpdateHotbarUI();
        SelectSlot(selectedSlot);
    }

    void Update()
    {
        HandleKeyInput();

        if (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
    }

    void HandleKeyInput()
    {
        if (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
                break;
            }
        }
    }

    void SelectSlot(int slotIndex)
    {
        if (selectedSlot < hotbarSlots.Length && hotbarSlots[selectedSlot] != null)
        {
            hotbarSlots[selectedSlot].color = Color.white;
        }

        selectedSlot = slotIndex;

        if (selectedSlot < hotbarSlots.Length && hotbarSlots[selectedSlot] != null)
        {
            hotbarSlots[selectedSlot].color = Color.yellow;
        }

        Debug.Log($"Вибрана комірка: {selectedSlot + 1}");

        if (selectedSlot == 0)
        {
            // Перевіряємо, чи є взагалі предмет у першому слоті, перш ніж екіпірувати
            if (hotbarItems[selectedSlot] != null && hotbarItems[selectedSlot].itemTag == "Weapon")
            {
                EquipWeapon.Instance.Equip();
                Debug.Log("WEAPON IS EQUIPED");
            }
            else
            {
                // Якщо в слоті 0 немає зброї, то розспоряджаємо поточну
                EquipWeapon.Instance.UnEquip();
                Debug.Log("NO WEAPON IN SLOT 1, UNEQUIPING CURRENT");
            }
        }
        else // Якщо вибрано будь-який інший слот, зброя знімається
        {
            EquipWeapon.Instance.UnEquip();
            Debug.Log("WEAPON IS UNEQUIPED");
        }
    }

    // Додає предмет до хотбару
    public bool AddItemToHotbar(Item itemToAdd)
    {
        if (!allowedHotbarTags.Contains(itemToAdd.itemTag))
        {
            Debug.LogWarning($"Предмет '{itemToAdd.itemName}' з тегом '{itemToAdd.itemTag}' не може бути доданий до Hotbar'у. Тег не дозволено.");
            return false;
        }

        if (itemToAdd.itemTag == "Weapon")
        {
            if (hotbarItems[0] == null)
            {
                hotbarItems[0] = itemToAdd;
                Debug.Log($"Предмет '{itemToAdd.itemName}' (зброя) додано до Hotbar'у в комірку 1.");
                UpdateHotbarUI();
                SelectSlot(0);
                return true;
            }
            else
            {
                Debug.Log($"Комірка 1 зайнята '{hotbarItems[0].itemName}'. Шукаємо вільну комірку для '{itemToAdd.itemName}'.");
            }
        }

        for (int i = 1; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = itemToAdd;
                Debug.Log($"Предмет '{itemToAdd.itemName}' додано до Hotbar'у в комірку {i + 1}.");
                UpdateHotbarUI();
                return true;
            }
        }

        Debug.LogWarning($"Hotbar повний! Неможливо додати предмет '{itemToAdd.itemName}'.");
        return false;
    }

    // Оновлює візуальне відображення слотів хотбару
    void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarItems[i] != null)
            {
                hotbarSlots[i].sprite = hotbarItems[i].itemIcon;
                hotbarSlots[i].enabled = true;
            }
            else
            {
                hotbarSlots[i].sprite = null;
                hotbarSlots[i].enabled = false;
            }
            hotbarSlots[i].color = (i == selectedSlot) ? Color.yellow : Color.white;
        }
    }

    // Використання вибраного предмета
    void UseSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < hotbarItems.Length)
        {
            Item itemToUse = hotbarItems[selectedSlot];
            if (itemToUse != null)
            {
                Debug.Log($"Використано предмет: {itemToUse.itemName}");
            }
            else
            {
                Debug.Log("Вибрана комірка порожня.");
            }
        }
    }
}
