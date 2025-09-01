using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HotbarManager : MonoBehaviour
{
    public Image[] hotbarSlots;
    public Item[] hotbarItems;
    public GameObject[] hotbarPhysicalObjects;

    public int selectedSlot = -1;
    public List<string> allowedHotbarTags = new List<string>();

    void Start()
    {
        hotbarItems = new Item[hotbarSlots.Length];
        hotbarPhysicalObjects = new GameObject[hotbarSlots.Length];
        UpdateHotbarUI();

        foreach (var obj in hotbarPhysicalObjects)
        {
            if (obj != null) obj.SetActive(false);
        }

        SelectSlot(0);
    }

    void Update()
    {
        if (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding)
            return;

        HandleKeyInput();
    }

    public void HandleKeyInput()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
                break;
            }
        }
    }

    public void SelectSlot(int slotIndex)
    {
        if (selectedSlot == slotIndex)
        {
            DeselectSlot(selectedSlot);
            selectedSlot = -1;
            Debug.Log("���� ������������.");
            return;
        }

        DeselectSlot(selectedSlot);
        selectedSlot = slotIndex;
        ActivateSlot(selectedSlot);
    }

    public void DeselectSlot(int slot)
    {
        if (slot < 0 || slot >= hotbarSlots.Length) return;

        if (hotbarSlots[slot] != null)
            hotbarSlots[slot].color = Color.white;

        if (hotbarPhysicalObjects[slot] != null)
        {
            hotbarPhysicalObjects[slot].SetActive(false);
            if (EquipManager.Instance != null && EquipManager.Instance.equippedItem == hotbarPhysicalObjects[slot])
                EquipManager.Instance.UnEquip();
        }
    }

    public void ActivateSlot(int slot)
    {
        if (hotbarSlots[slot] != null)
            hotbarSlots[slot].color = Color.yellow;

        Debug.Log($"������� ������: {slot + 1}");

        GameObject obj = hotbarPhysicalObjects[slot];
        Item item = hotbarItems[slot];

        if (obj != null)
        {
            obj.SetActive(true);
            //&& (item.itemTag == "Weapon" || item.itemTag == "AutoRifle"
            if (item != null )
            {
                if (EquipManager.Instance != null)
                {
                    EquipManager.Instance.Equip();
                }
                Debug.Log("����� ���������.");
            }
            else
            {
                if (EquipManager.Instance != null)
                    EquipManager.Instance.UnEquip();

                Debug.Log("�� ����� ��� ���� �������.");
            }
        }
        else
        {
            if (EquipManager.Instance != null)
                EquipManager.Instance.UnEquip();

            Debug.Log("������� ����. ������ ��������.");
        }
    }

    public bool AddItemToHotbar(Item itemToAdd, GameObject pickupObject)
    {
        if (!allowedHotbarTags.Contains(itemToAdd.itemTag))
        {
            Debug.LogWarning($"��� '{itemToAdd.itemTag}' �� ��������� ��� Hotbar.");
            return false;
        }

        // ��������: ���� ��� ������ � ���� 0 ��� ����� � �� ���������
        if ((itemToAdd.itemTag == "Weapon" || itemToAdd.itemTag == "AutoRifle") && hotbarItems[0] != null)
        {
            Debug.LogWarning($"��� � ����� � ������� ����! '{itemToAdd.itemName}' �� ���� �������.");
            return false;
        }

        int targetSlot = (itemToAdd.itemTag == "Weapon" || itemToAdd.itemTag == "AutoRifle") ? 0 : FindFreeSlot();

        if (targetSlot == -1)
        {
            Debug.LogWarning($"Hotbar ������! ��������� ������ '{itemToAdd.itemName}'.");
            return false;
        }

        hotbarItems[targetSlot] = itemToAdd;
        hotbarPhysicalObjects[targetSlot] = pickupObject;
        UpdateHotbarUI();

        if (EquipManager.Instance != null)
            EquipManager.Instance.AddItem(pickupObject);

        Debug.Log($"������� '{itemToAdd.itemName}' ������ �� ������ {targetSlot + 1}.");
        SelectSlot(targetSlot);
        return true;
    }
    int FindFreeSlot()
    {
        for (int i = 1; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
                return i;
        }
        return -1;
    }

    public void UpdateHotbarUI()
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

    /*void UseSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < hotbarItems.Length)
        {
            Item itemToUse = hotbarItems[selectedSlot];
            if (itemToUse != null)
            {
                Debug.Log($"����������� �������: {itemToUse.itemName}");
                // itemToUse.Use(); // ���� ����������
            }
            else
            {
                Debug.Log("������� ������ �������.");
            }
        }
    }*/
    public static HotbarManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // ������ �� ����������, ���� �����
    }

}