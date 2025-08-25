using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    private HotbarManager HotbarManager;
    private EquipManager EquipManager;

    private GameObject hotbarManagerObject;
    public GameObject handObject;
    public TextMeshProUGUI ammoText;
    private Transform handTransform;
    public float forwardOffset = 1.5f;
    public float throwForce = 5f;

    private void Awake()
    {
        hotbarManagerObject = GameObject.Find("GameManager(DON`T DELETE!!!)");

        HotbarManager = hotbarManagerObject.GetComponent<HotbarManager>();
        EquipManager = GetComponent<EquipManager>();
        handTransform = gameObject.transform;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.G))
        {
            DropSelectedItem();
        }
    }
    public void DropSelectedItem()
    {
        int slot = HotbarManager.selectedSlot;

        GameObject itemObject = HotbarManager.hotbarPhysicalObjects[slot];
        Item itemData = HotbarManager.hotbarItems[slot];

        EquipManager.UnEquip();

        HotbarManager.hotbarItems[slot] = null;
        HotbarManager.hotbarPhysicalObjects[slot] = null;
        HotbarManager.UpdateHotbarUI();
        HotbarManager.DeselectSlot(slot);

        // Перемещаем предмет вперёд от руки
        itemObject.transform.SetParent(null);
        itemObject.transform.position = handTransform.position + handTransform.forward * forwardOffset;
        itemObject.SetActive(true);
        itemObject.GetComponent<Equipment>().isDropped = true;

        itemObject.GetComponent<Equipment>().isAdded = false;
        EquipManager.equippedItem = null;

        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = handTransform.forward * throwForce;
        }

        ammoText.text = ("0 / 0");
        Debug.Log($"Викинуто: {itemData.itemName}");
    }
}