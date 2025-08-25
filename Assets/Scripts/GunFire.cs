using UnityEngine;
using System.Collections;
using TMPro;

public class GunFire : Equipment, IEquipment
{
    public static GunFire Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        isDropped = true;
        totalAmmo = stats.maxAmmo;
        currentAmmoInMagazine = stats.magazineSize;
    }

    public WeaponStats stats;
    public Transform firePoint;
    public Camera virtualCamera;

    [Header("GameObject")]
    public GameObject hitEffect;
    public GameObject bulletTrailPrefab;
    public GameObject Player;

    private KeyCode _shootKey;
    private KeyCode _reloadKey;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    [Header("Ammo")]
    public float currentAmmoInMagazine;
    public float totalAmmo;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;


    public void OnAdd()
    {
        isAdded = true;
        isEquiped = false;

        Finder();
        
        isDropped = false;
        UpdateKeybinds();
        UpdateUI();
    }

    public void OnEquip()
    {
        isEquiped = true;
        UpdateUI();
    }

    public void OnUnEquip()
    {
        isEquiped = false;
        UpdateUI();
    }

    void Update()
    {
        UpdateKeybinds();

        if (isReloading || !isEquiped) return;

        if ((currentAmmoInMagazine <= 0 || Input.GetKeyDown(_reloadKey)) && currentAmmoInMagazine < stats.magazineSize && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKey(_shootKey) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + stats.fireRate;
            Shoot();
        }
    }

    void UpdateKeybinds()
    {
        if (KeybindManager.Instance != null)
        {
            _shootKey = KeybindManager.Instance.GetKey("Shoot");
            _reloadKey = KeybindManager.Instance.GetKey("Reload");
        }
    }

    IEnumerator Reload()
    {
        if (isReloading || currentAmmoInMagazine == stats.magazineSize || totalAmmo <= 0)
            yield break;

        isReloading = true;
        Debug.Log("🔄 Перезарядка...");

        yield return new WaitForSeconds(stats.reloadTime);

        float neededAmmo = stats.magazineSize - currentAmmoInMagazine;
        float ammoToLoad = Mathf.Min(neededAmmo, totalAmmo);

        currentAmmoInMagazine += ammoToLoad;
        totalAmmo -= ammoToLoad;

        UpdateUI();
        isReloading = false;
    }

    void Shoot()
    {
        if (currentAmmoInMagazine <= 0) return;

        Debug.Log("Shoot!");
        currentAmmoInMagazine--;
        UpdateUI();

        Ray ray = virtualCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint = firePoint.position + ray.direction * stats.range;

        if (Physics.Raycast(ray, out RaycastHit hit, stats.range))
        {
            hitPoint = hit.point;
            Debug.Log("Попадание по: " + hit.transform.name);

            if (hitEffect != null)
                Destroy(Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)), 1f);

            if (hit.transform.TryGetComponent(out PlayerStats target))
            {
                target.TakeDamage(stats.damage);
                Debug.Log("- health");
            }
        }

        StartCoroutine(SpawnTracer(firePoint.position, hitPoint, stats.spread));
    }

    IEnumerator SpawnTracer(Vector3 start, Vector3 end, float spread)
    {
        GameObject trailGO = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        TrailRenderer trail = trailGO.GetComponent<TrailRenderer>();

        float speed = 200f;
        float distance = Vector3.Distance(start, end);
        float travelTime = distance / speed;

        Vector3 direction = (end - start).normalized + new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );

        Vector3 finalEnd = start + direction.normalized * distance;

        float elapsed = 0f;
        while (elapsed < travelTime)
        {
            trailGO.transform.position = Vector3.Lerp(start, finalEnd, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        trailGO.transform.position = finalEnd;
        Destroy(trailGO, trail.time);
    }

    public void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmoInMagazine} / {totalAmmo}";
    }

    public void Finder()
    {
        virtualCamera = GameObject.Find("MainCamera")?.GetComponent<Camera>();
        ammoText = GameObject.Find("Ammotext")?.GetComponent<TextMeshProUGUI>();
        Player = GameObject.Find("Player");

        if (virtualCamera == null)
            Debug.LogError("GunFire: 'MainCamera' not found!");

        if (ammoText == null)
            Debug.LogWarning("GunFire: 'Ammotext' not found. Ammo UI will not be shown.");

        if (Player == null)
            Debug.LogError("GunFire: 'Player' not found!");
    }
}