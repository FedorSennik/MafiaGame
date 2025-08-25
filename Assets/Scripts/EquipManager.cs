using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    [Header("Об'єкти")]
    public GameObject equippedItem;
    public GameObject hand;
    public GameObject back;

    private IEquipment equipmentScript;

    public void Equip()
    {
        if (equippedItem == null)
        {
            Debug.LogWarning("EquipManager: Немає предмета для екіпірування.");
            return;
        }

        if (equipmentScript == null)
        {
            Finder();
            if (equipmentScript == null) return;
        }

        equippedItem.transform.SetParent(hand.transform);
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.SetActive(true);

        equipmentScript.OnEquip();
    }

    public void UnEquip()
    {
        if (equippedItem == null || equipmentScript == null) return;

        equippedItem.transform.SetParent(back.transform);
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.SetActive(false);

        equipmentScript.OnUnEquip();
    }

    public void AddItem(GameObject item)
    {
        if (equippedItem != null && equippedItem != item)
        {
            UnEquip();
        }

        equippedItem = item;
        Finder();

        equippedItem.transform.SetParent(back.transform);
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.SetActive(false);

        equipmentScript?.OnAdd();
    }

    private void Finder()
    {
        if (equippedItem != null)
        {
            equipmentScript = equippedItem.GetComponent<IEquipment>();
            if (equipmentScript == null)
                Debug.LogError("EquipManager: Об'єкт не реалізує IEquipment!");
        }
        else
        {
            Debug.LogWarning("EquipManager: Немає об'єкта для Finder.");
        }
    }
}