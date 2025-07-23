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
    [Header("Œ·'∫ÍÚË")]
    public GameObject equipedGun;
    public GameObject hand;
    public GameObject back;
    private GunFire firescript;

    public bool isAdded;

    private void Start()
    {
        isAdded = false;
    }

    public void Equip()
    {
        equipedGun.transform.SetParent(hand.transform);
        equipedGun.transform.localPosition = Vector3.zero;
        equipedGun.transform.localRotation = Quaternion.identity;
        firescript.isEquiped = true;
    }

    public void UnEquip()
    {
        equipedGun.transform.SetParent(back.transform);
        equipedGun.transform.localPosition = Vector3.zero;
        equipedGun.transform.localRotation = Quaternion.identity;
        firescript.isEquiped = false;
    }

    public void AddGun(GameObject gun)
    {
        equipedGun = gun;
        Finder();
        equipedGun.transform.SetParent(back.transform);

        equipedGun.transform.localPosition = Vector3.zero;
        equipedGun.transform.localRotation = Quaternion.identity;
        firescript.isEquiped = false;
        isAdded = true;
    }

    public void Finder()
    {
        firescript = equipedGun.GetComponent<GunFire>();
    }
}
