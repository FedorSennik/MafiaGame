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
            Debug.LogError("Об'єкт гравця з тегом 'Player' не знайдено на сцені! Підбір предметів не працюватиме.");
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
                    Debug.Log($"Ви можете підібрати {item.itemName}. Натисніть {pickupKey}.");
                    // TODO: Можна додати візуальний фідбек, наприклад, підсвічування предмета або підказку UI
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
                    Debug.Log("Ви вийшли з радіуса підбору.");
                    // TODO: Прибрати візуальний фідбек
                }
            }
        }
    }

    void PickUp()
    {
        HotbarManager hotbarManager = FindObjectOfType<HotbarManager>();

        if (hotbarManager == null)
        {
            Debug.LogError("HotbarManager не знайдено на сцені! Переконайтеся, що він присутній.");
            return;
        }

        bool wasPickedUp = hotbarManager.AddItemToHotbar(item);

        if (wasPickedUp)
        {
            Debug.Log($"Підібрано: {quantity} x {item.itemName}");
            if (item.itemTag == "Weapon")
            {
                EquipWeapon.Instance.AddGun(gameObject);
            }
            
        }
        else
        {
            Debug.Log($"Не вдалося підібрати {item.itemName}. Hotbar повний або тег не дозволено.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}