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
    [Header("��'����")]
    public GameObject equipedGun;
    public GameObject hand;
    public GameObject back;
    public GunFire firescript;

    // ����� ��� ���������� ���� (���������� � ����)
    public void Equip()
    {
        if (equipedGun == null)
        {
            Debug.LogWarning("EquipWeapon: ���� ���� ��� ����������.");
            return;
        }
        if (firescript == null)
        {
            Debug.LogError("EquipWeapon: firescript �� �������� ��� equipedGun. ������ ��������� Finder().");
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

    // ����� ��� ������ ���� (���������� �� �����)
    public void UnEquip()
    {
        if (equipedGun == null)
        {
            // Debug.LogWarning("EquipWeapon: ���� ���� ��� ������.");
            return;
        }
        if (firescript == null)
        {
            // Debug.LogWarning("EquipWeapon: firescript �� �������� ��� equipedGun ��� UnEquip.");
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

    // ����� ��� ����������� ������� GunFire �� ��������� ����
    public void Finder()
    {
        if (equipedGun != null)
        {
            firescript = equipedGun.GetComponent<GunFire>();
            if (firescript == null)
            {
                Debug.LogError("EquipWeapon: �� ���������� ���� ���� ���������� GunFire!");
            }
        }
        else
        {
            Debug.LogWarning("EquipWeapon: ���� ��'���� equipedGun ��� Finder.");
        }
    }
}
