using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemTag;
    [TextArea(3, 5)]
    public string itemDescription;

    [Header("Shop & Spawning")]
    public int itemPrice = 100;
    // !!! ¬ажливо: ÷е Prefab 3D-модел≥, €кий буде спавнитись на сцен≥
    // ѕерет€гн≥ть сюди Prefab, на €кому Ї компонент ItemPickup
    public GameObject spawnablePrefab;
}
