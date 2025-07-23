using UnityEngine;


[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public string itemTag;

    public WeaponStats stats;
    // �����������
    public Item(string name, Sprite icon, string tag, WeaponStats stats)
    {
        itemName = name;
        itemIcon = icon;
        itemTag = tag;
        this.stats = stats;
    }
}