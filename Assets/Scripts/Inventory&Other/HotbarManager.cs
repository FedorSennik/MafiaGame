using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HotbarManager : MonoBehaviour
{
    public Image[] hotbarSlots;
    public Item[] hotbarItems;

    public int selectedSlot = 0;

    public List<Item> availableItems = new List<Item>();

    void Start()
    {
        hotbarItems = new Item[5];

        UpdateHotbarUI();
    }

    void Update()
    {
        HandleKeyInput();

        if (Input.GetKeyDown(KeyCode.E))
        {
            SimulateItemPickup();
        }
    }

    void HandleKeyInput()
    {
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
    }

    public void AddItemToHotbar(Item itemToAdd)
    {

        if (selectedSlot == 0)
        {
            if (itemToAdd.itemTag == "Weapon")
            {
                hotbarItems[0] = itemToAdd;
                Debug.Log($"Предмет '{itemToAdd.itemName}' додано до Hotbar'у в комірку 1 (зброя).");
            }
            else
            {
                Debug.Log($"Увага: Комірка 1 призначена тільки для зброї. Предмет '{itemToAdd.itemName}' не може бути доданий сюди.");
                return;
            }
        }
        else
        {
            if (hotbarItems[selectedSlot] == null)
            {
                hotbarItems[selectedSlot] = itemToAdd;
                Debug.Log($"Предмет '{itemToAdd.itemName}' додано до Hotbar'у в комірку {selectedSlot + 1}.");
            }
            else
            {
                Debug.Log($"Комірка {selectedSlot + 1} вже зайнята предметом '{hotbarItems[selectedSlot].itemName}'.");
            }
        }

        UpdateHotbarUI();
    }

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
        }
    }

    void SimulateItemPickup()
    {
        if (availableItems.Count == 0)
        {
            Debug.LogWarning("Немає доступних предметів для підбору в availableItems List!");
            return;
        }

        // Вибираємо випадковий предмет для підбору
        Item randomItem = availableItems[Random.Range(0, availableItems.Count)];
        Debug.Log($"Спроба підібрати предмет: {randomItem.itemName} (Тег: {randomItem.itemTag})");
        AddItemToHotbar(randomItem);
    }
}