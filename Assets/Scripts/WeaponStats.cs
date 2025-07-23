using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public GameObject weaponModel;
    public string weaponName = "�������";
    public float damage = 25f;
    public float fireRate = 0.2f; // ����� ����� ����������
    public float range = 100f;
    public float magazineSize = 30;
    public float maxAmmo;
    public float reloadTime = 2f;
    public float spread;
}