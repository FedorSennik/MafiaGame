using UnityEngine;


[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public string itemTag;

    public WeaponStats stats;
    // Конструктор
    public Item(string name, Sprite icon, string tag, WeaponStats stats)
    {
        itemName = name;
        itemIcon = icon;
        itemTag = tag;
        this.stats = stats;
    }
}