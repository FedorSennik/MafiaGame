using UnityEngine;

public class HealItem : MonoBehaviour, IEquipment
{
    public void OnEquip()
    {
        Debug.Log("HealItem: готовий до використання.");
    }

    public void OnUnEquip()
    {
        Debug.Log("HealItem: прибрано.");
    }

    public void OnAdd()
    {
        Debug.Log("HealItem: додано до інвентаря.");
    }
}