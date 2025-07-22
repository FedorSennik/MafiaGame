using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Security.Cryptography;
using Cinemachine;

public class GunFire : MonoBehaviour
{
    public WeaponStats stats;
    public Transform firePoint;
    //public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public Camera VirtualCamera;
    public GameObject bulletTrailPrefab; // Префаб трейла для визуализации трассера

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
        Debug.Log("Перезарядка...");
        yield return new WaitForSeconds(stats.reloadTime);
        currentAmmo = stats.magazineSize;
        isReloading = false;
    }

    void Shoot()
    {
        Debug.Log("Shoot");
        //muzzleFlash?.Play();
        currentAmmo--;

        Ray ray = VirtualCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 hitPoint = firePoint.position + ray.direction * stats.range;

        if (Physics.Raycast(ray, out hit, stats.range))
        {
            hitPoint = hit.point;
            Debug.Log("Попадание по: " + hit.transform.name);

            if (hitEffect != null)
            {
                GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }

            PlayerStats target = hit.transform.GetComponent<PlayerStats>();
            if (target != null)
            {
                target.TakeDamage(stats.damage);
                Debug.Log("- health");
            }
        }

        // Визуализация трассера
        StartCoroutine(SpawnTracer(firePoint.position, hitPoint));
    }

    IEnumerator SpawnTracer(Vector3 start, Vector3 end)
    {
        GameObject trailGO = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        TrailRenderer trail = trailGO.GetComponent<TrailRenderer>();

        float speed = 200f;
        float distance = Vector3.Distance(start, end);
        float travelTime = distance / speed;

        float elapsed = 0f;
        while (elapsed < travelTime)
        {
            trailGO.transform.position = Vector3.Lerp(start, end, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        trailGO.transform.position = end;
        Destroy(trailGO, trail.time); // Удалить после исчезновения трейла
    }
}