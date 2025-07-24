using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HotbarManager : MonoBehaviour
{
    public Image[] hotbarSlots;
    public Item[] hotbarItems;

    public GameObject[] hotbarPhysicalObjects;

    public int selectedSlot = 0;

    public List<string> allowedHotbarTags = new List<string>();

    void Start()
    {
        hotbarItems = new Item[5];
        hotbarPhysicalObjects = new GameObject[5];
        UpdateHotbarUI();

        foreach (GameObject obj in hotbarPhysicalObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        SelectSlot(selectedSlot);
    }

    void Update()
    {
        HandleKeyInput();

        if (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding)
        {
            return;
        }

        /*if (Input.GetMouseButtonDown(0)) // ���������� ��� ������ ���� ��� ������������
        {
            UseSelectedItem();
        }*/
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

        if (selectedSlot < hotbarPhysicalObjects.Length && hotbarPhysicalObjects[selectedSlot] != null)
        {
            hotbarPhysicalObjects[selectedSlot].SetActive(false);
            if (EquipWeapon.Instance != null && EquipWeapon.Instance.equipedGun == hotbarPhysicalObjects[selectedSlot])
            {
                EquipWeapon.Instance.UnEquip();
            }
        }

        selectedSlot = slotIndex;

        if (selectedSlot < hotbarSlots.Length && hotbarSlots[selectedSlot] != null)
        {
            hotbarSlots[selectedSlot].color = Color.yellow;
        }

        Debug.Log($"������� ������: {selectedSlot + 1}");

        if (selectedSlot < hotbarPhysicalObjects.Length && hotbarPhysicalObjects[selectedSlot] != null)
        {
            hotbarPhysicalObjects[selectedSlot].SetActive(true);

            if (selectedSlot == 0 && (hotbarItems[selectedSlot].itemTag == "Weapon" || hotbarItems[selectedSlot].itemTag == "AutoRifle"))
            {
                if (EquipWeapon.Instance != null)
                {
                    EquipWeapon.Instance.equipedGun = hotbarPhysicalObjects[selectedSlot];
                    EquipWeapon.Instance.Finder();
                    EquipWeapon.Instance.Equip();
                }
                Debug.Log("WEAPON IS EQUIPED");
            }
            else
            {
                if (EquipWeapon.Instance != null && EquipWeapon.Instance.firescript != null && EquipWeapon.Instance.firescript.isEquiped)
                {
                    EquipWeapon.Instance.UnEquip();
                }
                Debug.Log("WEAPON IS UNEQUIPED or NO WEAPON IN SLOT");
            }
        }
        else
        {
            if (EquipWeapon.Instance != null && EquipWeapon.Instance.firescript != null && EquipWeapon.Instance.firescript.isEquiped)
            {
                EquipWeapon.Instance.UnEquip();
            }
            Debug.Log("Selected empty slot. Unequiping any active weapon.");
        }
    }

    // ���� ������� �� �������
    public bool AddItemToHotbar(Item itemToAdd, GameObject pickupObject)
    {
        if (!allowedHotbarTags.Contains(itemToAdd.itemTag))
        {
            Debug.LogWarning($"������� '{itemToAdd.itemName}' � ����� '{itemToAdd.itemTag}' �� ���� ���� ������� �� Hotbar'�. ��� �� ���������.");
            return false;
        }

        if (itemToAdd.itemTag == "Weapon" || itemToAdd.itemTag == "AutoRifle")
        {
            if (hotbarItems[0] == null)
            {
                hotbarItems[0] = itemToAdd;
                hotbarPhysicalObjects[0] = pickupObject;
                Debug.Log($"������� '{itemToAdd.itemName}' (�����) ������ �� Hotbar'� � ������ 1.");
                UpdateHotbarUI();

                if (EquipWeapon.Instance != null)
                {
                    EquipWeapon.Instance.AddGun(pickupObject);
                }

                SelectSlot(0);
                return true;
            }
            else
            {
                Debug.Log($"������ 1 ������� '{hotbarItems[0].itemName}'. ������ ����� ������ ��� '{itemToAdd.itemName}'.");
            }
        }

        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = itemToAdd;
                hotbarPhysicalObjects[i] = pickupObject;
                Debug.Log($"������� '{itemToAdd.itemName}' ������ �� Hotbar'� � ������ {i + 1}.");
                UpdateHotbarUI();
                return true;
            }
        }

        Debug.LogWarning($"Hotbar ������! ��������� ������ ������� '{itemToAdd.itemName}'.");
        return false;
    }

    // ������� �������� ����������� ����� �������
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

    // !! �������� �����: ����� ��� ��������������.
    /*void UseSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < hotbarItems.Length)
        {
            Item itemToUse = hotbarItems[selectedSlot];
            if (itemToUse != null)
            {
                Debug.Log($"����������� �������: {itemToUse.itemName}");
                // ��� ���� ���� ��������� ����� ������������ ��������
                // ���������, ������ ������ Use() �� ������� ��� ������������ ������
            }
            else
            {
                Debug.Log("������� ������ �������.");
            }
        }
    }*/
}
