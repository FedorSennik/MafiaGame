using UnityEngine;

public class HealItem : MonoBehaviour, IEquipment
{
    public void OnEquip()
    {
        Debug.Log("HealItem: ������� �� ������������.");
    }

    public void OnUnEquip()
    {
        Debug.Log("HealItem: ��������.");
    }

    public void OnAdd()
    {
        Debug.Log("HealItem: ������ �� ���������.");
    }
}