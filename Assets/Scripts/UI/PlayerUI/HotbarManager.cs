using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HotbarManager : MonoBehaviour
{
    public Image[] hotbarSlots;
    public Item[] hotbarItems;


    public int selectedSlot = 0;

    // ������ ���������� ���� ��� ��������, �� ������ ��������� � Hotbar
    // ������������ � Inspector.
    public List<string> allowedHotbarTags = new List<string>();

    void Start()
    {
        hotbarItems = new Item[5];
        UpdateHotbarUI();
        SelectSlot(selectedSlot);
    }

    void Update()
    {
        // ������� ���������� ����� ��� ������ ������ (1-5)
        HandleKeyInput();

        // ��������� �����: ������������ �������� � ��������� �����
        /*if (Input.GetMouseButtonDown(0)) // ���������� ��� ������ ���� ��� ������������
        {
            UseSelectedItem();
        }*/
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

        Debug.Log($"������� ������: {selectedSlot + 1}");

        if (selectedSlot == 0)
        {
            EquipWeapon.Instance.Equip();
            Debug.Log("WEPON IS EQUIP");
        }
        else
        {
            EquipWeapon.Instance.UnEquip();
            Debug.Log("WEPON IS unEQUIP");
        }
    }

    public bool AddItemToHotbar(Item itemToAdd)
    {
        if (!allowedHotbarTags.Contains(itemToAdd.itemTag))
        {
            Debug.LogWarning($"������� '{itemToAdd.itemName}' � ����� '{itemToAdd.itemTag}' �� ���� ���� ������� �� Hotbar'�. ��� �� ���������.");
            return false;
        }

        if (itemToAdd.itemTag == "Weapon")
        {
            if (hotbarItems[0] == null)
            {
                hotbarItems[0] = itemToAdd;
                Debug.Log($"������� '{itemToAdd.itemName}' (�����) ������ �� Hotbar'� � ������ 1.");
                UpdateHotbarUI();
                return true;
            }
            else
            {
                Debug.Log($"������ 1 ������� '{hotbarItems[0].itemName}'. ������ ����� ������ ��� '{itemToAdd.itemName}'.");
            }
        }

        for (int i = 1; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = itemToAdd;
                Debug.Log($"������� '{itemToAdd.itemName}' ������ �� Hotbar'� � ������ {i + 1}.");
                UpdateHotbarUI();
                return true;
            }
        }

        Debug.LogWarning($"Hotbar ������! ��������� ������ ������� '{itemToAdd.itemName}'.");
        return false;
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
            hotbarSlots[i].color = (i == selectedSlot) ? Color.yellow : Color.white;
        }
    }
}
    /*void UseSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < hotbarItems.Length)
        {
            Item itemToUse = hotbarItems[selectedSlot];
            if (itemToUse != null)
            {
                Debug.Log($"����������� �������: {itemToUse.itemName}");
            }
            else
            {
                Debug.Log("������� ������ �������.");
            }
        }
    }
}*/