using UnityEngine;


[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public string itemTag;

    // Конструктор
    public Item(string name, Sprite icon, string tag)
    {
        itemName = name;
        itemIcon = icon;
        itemTag = tag;
    }
}