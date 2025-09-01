using UnityEngine;
using System.Collections.Generic;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity = 1;
    public float pickupRadius = 3f;
    public KeyCode pickupKey = KeyCode.E;

    private static List<ItemPickup> pickupsInRange = new List<ItemPickup>();
    private static GameObject player;

    private bool isInRange = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Об'єкт гравця з тегом 'Player' не знайдено на сцені! Підбір предметів не працюватиме.");
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= pickupRadius)
        {
            if (!isInRange)
            {
                isInRange = true;
                pickupsInRange.Add(this);
                Debug.Log($"Ви можете підібрати {item.itemName}. Натисніть {pickupKey}.");
                // TODO: Візуальний фідбек
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                pickupsInRange.Remove(this);
                Debug.Log("Ви вийшли з радіуса підбору.");
                // TODO: Прибрати фідбек
            }
        }

        // Обработка ввода только один раз на кадр
        if (this == GetClosestPickup() && Input.GetKeyDown(pickupKey))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        if (GetComponent<Equipment>()?.isDropped == true)
        {
            HotbarManager hotbarManager = FindObjectOfType<HotbarManager>();
            if (hotbarManager == null)
            {
                Debug.LogError("HotbarManager не знайдено на сцені!");
                return;
            }

            bool wasPickedUp = hotbarManager.AddItemToHotbar(item, gameObject);
            if (wasPickedUp)
            {
                Debug.Log($"Підібрано: {quantity} x {item.itemName}");
                gameObject.SetActive(false);
                pickupsInRange.Remove(this);
            }
            else
            {
                Debug.Log($"Не вдалося підібрати {item.itemName}.");
            }
        }
    }

    private static ItemPickup GetClosestPickup()
    {
        ItemPickup closest = null;
        float minDist = float.MaxValue;

        foreach (var pickup in pickupsInRange)
        {
            float dist = Vector3.Distance(player.transform.position, pickup.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = pickup;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}