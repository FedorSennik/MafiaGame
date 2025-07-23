using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity = 1;
    public float pickupRadius = 3f;
    public KeyCode pickupKey = KeyCode.E;

    private GameObject player;
    private bool isInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("��'��� ������ � ����� 'Player' �� �������� �� ����! ϳ��� �������� �� �����������.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= pickupRadius)
            {
                if (!isInRange)
                {
                    isInRange = true;
                    Debug.Log($"�� ������ ������� {item.itemName}. �������� {pickupKey}.");
                    // TODO: ����� ������ ��������� ������, ���������, ����������� �������� ��� ������� UI
                }

                if (Input.GetKeyDown(pickupKey))
                {
                    PickUp();
                }
            }
            else
            {
                if (isInRange)
                {
                    isInRange = false;
                    Debug.Log("�� ������ � ������ ������.");
                    // TODO: �������� ��������� ������
                }
            }
        }
    }

    void PickUp()
    {
        HotbarManager hotbarManager = FindObjectOfType<HotbarManager>();

        if (hotbarManager == null)
        {
            Debug.LogError("HotbarManager �� �������� �� ����! �������������, �� �� ��������.");
            return;
        }

        bool wasPickedUp = hotbarManager.AddItemToHotbar(item);

        if (wasPickedUp)
        {
            Debug.Log($"ϳ������: {quantity} x {item.itemName}");
            if (item.itemTag == "Weapon")
            {
                EquipWeapon.Instance.AddGun(gameObject);
            }
            
        }
        else
        {
            Debug.Log($"�� ������� ������� {item.itemName}. Hotbar ������ ��� ��� �� ���������.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}