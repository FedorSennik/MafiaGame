using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public GameObject weaponModel;
    public string weaponName = "Автомат";
    public int damage = 25;
    public float fireRate = 0.2f; // Время между выстрелами
    public float range = 100f;
    public float magazineSize = 30;
    public float maxAmmo;
    public float reloadTime = 2f;
    public float spread;
}