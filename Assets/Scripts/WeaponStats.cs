using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public GameObject weaponModel;
    public string weaponName = "Автомат";
    public float damage = 25f;
    public float fireRate = 0.2f; // Время между выстрелами
    public float range = 100f;
    public int magazineSize = 30;
    public float reloadTime = 2f;
}