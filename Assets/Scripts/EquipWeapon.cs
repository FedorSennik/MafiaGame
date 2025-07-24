using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    public static EquipWeapon Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    [Header("Об'єкти")]
    public GameObject equipedGun;
    public GameObject hand;
    public GameObject back;
    public GunFire firescript;

    // Метод для екіпірування зброї (переміщення в руку)
    public void Equip()
    {
        if (equipedGun == null)
        {
            Debug.LogWarning("EquipWeapon: Немає зброї для екіпірування.");
            return;
        }
        if (firescript == null)
        {
            Debug.LogError("EquipWeapon: firescript не знайдено для equipedGun. Спроба викликати Finder().");
            Finder();
            if (firescript == null) return;
        }


        equipedGun.transform.SetParent(hand.transform);
        equipedGun.transform.localPosition = Vector3.zero;
        equipedGun.transform.localRotation = Quaternion.identity;
        equipedGun.SetActive(true);
        firescript.isEquiped = true;
        firescript.UpdateUI();
    }

    // Метод для зняття зброї (переміщення за спину)
    public void UnEquip()
    {
        if (equipedGun == null)
        {
            // Debug.LogWarning("EquipWeapon: Немає зброї для зняття.");
            return;
        }
        if (firescript == null)
        {
            // Debug.LogWarning("EquipWeapon: firescript не знайдено для equipedGun при UnEquip.");
            return;
        }

        equipedGun.transform.SetParent(back.transform);
        equipedGun.transform.localPosition = Vector3.zero;
        equipedGun.transform.localRotation = Quaternion.identity;
        equipedGun.SetActive(false);
        firescript.isEquiped = false;
        firescript.UpdateUI();
    }

    public void AddGun(GameObject gun)
    {
        if (equipedGun != null && equipedGun != gun)
        {
            UnEquip();
        }

        equipedGun = gun;
        Finder();

        equipedGun.transform.SetParent(back.transform);
        equipedGun.transform.localPosition = Vector3.zero;
        equipedGun.transform.localRotation = Quaternion.identity;
        equipedGun.SetActive(false);

        firescript.isEquiped = false;
        firescript.isAdded = true;
        firescript.UpdateUI();
    }

    // Метод для знаходження скрипта GunFire на екіпірованій зброї
    public void Finder()
    {
        if (equipedGun != null)
        {
            firescript = equipedGun.GetComponent<GunFire>();
            if (firescript == null)
            {
                Debug.LogError("EquipWeapon: На призначеній зброї немає компонента GunFire!");
            }
        }
        else
        {
            Debug.LogWarning("EquipWeapon: Немає об'єкта equipedGun для Finder.");
        }
    }
}
