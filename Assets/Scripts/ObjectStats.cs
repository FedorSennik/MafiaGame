using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectStats", menuName = "Inventory/Object")]
public class ObjectStats : ScriptableObject
{
    public float price;
    public float timeToPickup;
    public float objectWeight;
}