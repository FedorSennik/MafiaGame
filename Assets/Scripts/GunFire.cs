using UnityEngine;
using System.Collections;
using TMPro;

public class GunFire : MonoBehaviour
{
    public static GunFire Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public WeaponStats stats;
    public Transform firePoint;
    public GameObject hitEffect;
    public Camera virtualCamera;
    public GameObject bulletTrailPrefab;

    public KeyCode reloadKey = KeyCode.R;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    [Header("Ammo")]
    public float currentAmmoInMagazine;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        UpdateUI();
        Finder();
    }

    void Update()
    {
        if (isReloading || PlayerStats.Instance == null) return;

        if (currentAmmoInMagazine <= 0 || Input.GetKeyDown(reloadKey))
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

        if (PlayerStats.Instance.totalAmmo <= stats.magazineSize)
        {
            currentAmmoInMagazine = PlayerStats.Instance.totalAmmo;
            PlayerStats.Instance.totalAmmo = 0;
        }
        else if (PlayerStats.Instance.totalAmmo > stats.magazineSize)
        {
            currentAmmoInMagazine = stats.magazineSize;
            PlayerStats.Instance.totalAmmo =- stats.magazineSize;
        }
        else
        {
            Debug.Log("Error Reload");
        }
        UpdateUI();

        isReloading = false;
    }

    void Shoot()
    {
        Debug.Log("Shoot");

        currentAmmoInMagazine--;
        UpdateUI();

        Ray ray = virtualCamera.ScreenPointToRay(Input.mousePosition);
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
        Destroy(trailGO, trail.time);
    }

    public void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmoInMagazine} / {PlayerStats.Instance.totalAmmo}";
    }

    public void Finder()
    {
        GameObject camObj = GameObject.Find("MainCamera");
        virtualCamera = camObj.GetComponent<Camera>();
        GameObject textObj = GameObject.Find("Ammotext");
        ammoText = textObj.GetComponent<TextMeshProUGUI>();
    }
}