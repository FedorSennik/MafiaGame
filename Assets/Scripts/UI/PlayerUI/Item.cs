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
    // !!! �������: �� Prefab 3D-�����, ���� ���� ���������� �� ����
    // ���������� ���� Prefab, �� ����� � ��������� ItemPickup
    public GameObject spawnablePrefab;
}
