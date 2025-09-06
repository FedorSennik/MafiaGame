using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    private void Awake()
    {
        Instance ??= this;
    }

    [Header("Œ·'∫ÍÚË")]
    public GameObject equippedItem;
    public GameObject hand;
    public GameObject back;

    private IEquipment IequipmentScript;
    private Equipment equipmentScript;

    public void Equip(GameObject item)
    {
        equippedItem = item;
        Finder();
        if (equipmentScript.isDropped)
        {
            return;
        }

        equippedItem.transform.SetParent(hand.transform);
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;

        IequipmentScript.OnEquip();
    }

    public void UnEquip(GameObject item)
    {
        equippedItem = item;
        Finder();

        equippedItem.transform.SetParent(back.transform);
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;

        IequipmentScript.OnUnEquip();
    }

    public void AddItem(GameObject item)
    {
        if (equippedItem != item)
            UnEquip(item);

        equippedItem = item;
        Finder();

        equippedItem.transform.SetParent(back.transform);
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;

        IequipmentScript.OnAdd();
    }

    private void Finder()
    {
        IequipmentScript = equippedItem.GetComponent<IEquipment>();
        equipmentScript = equippedItem.GetComponent<Equipment>();
    }
}