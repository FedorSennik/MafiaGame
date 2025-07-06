using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class GunFire : MonoBehaviour
{
    public WeaponStats stats;
    public Transform firePoint;
// public ParticleSystem muzzleFlash;
    public GameObject hitEffect;

    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = stats.magazineSize;
    }

    void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + stats.fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("�����������...");
        yield return new WaitForSeconds(stats.reloadTime);
        currentAmmo = stats.magazineSize;
        isReloading = false;
    }

    void Shoot()
    {
        Debug.Log("Shoot");
       // muzzleFlash?.Play();
        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, stats.range))
        {
            Debug.Log("��������� ��: " + hit.transform.name);

            // ������ ���������
            if (hitEffect != null)
            {
                GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }

            // ���� ����
            PlayerStats target = hit.transform.GetComponent<PlayerStats>();
            if (target != null)
            {
                target.HealthChange(-stats.damage);
                Debug.Log("- health");
            }
        }
    }
}
