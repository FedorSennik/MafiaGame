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
    }

    public WeaponStats stats;
    public Transform firePoint;
    public Camera virtualCamera;

    [Header("GameObject")]
    public GameObject hitEffect;
    public GameObject bulletTrailPrefab;
    public GameObject Player;

    [Header("Input")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    [Header("Ammo")]
    public float currentAmmoInMagazine;
    public float totalAmmo;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    private void Update()
    {
        if (isEquiped)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                Fire();
            }

            if (Input.GetKeyDown(reloadKey))
            {
                Reload();
            }
        }
    }

    public void OnAdd()
    {
        isAdded = true;
        isEquiped = false;
        isDropped = false;

        if (stats != null)
        {
            totalAmmo = stats.maxAmmo;
            currentAmmoInMagazine = stats.magazineSize;
        }

        gameObject.SetActive(false);
        Destroy(GetComponent<Rigidbody>());
    }

    public void OnEquip()
    {
        isEquiped = true;
        gameObject.SetActive(true);
        Finder();
        UpdateUI();
    }

    public void OnUnEquip()
    {
        isEquiped = false;
        gameObject.SetActive(false);
    }

    public void OnRemove()
    {
        isAdded = false;
        isEquiped = false;
        isDropped = true;
        gameObject.SetActive(true);
        gameObject.AddComponent<Rigidbody>();
    }

    public void Fire()
    {
        if (isReloading || Time.time < nextTimeToFire)
        {
            return;
        }

        if (currentAmmoInMagazine <= 0)
        {
            Debug.Log("Немає патронів, потрібно перезарядитися!");
            return;
        }

        currentAmmoInMagazine--;
        nextTimeToFire = Time.time + stats.fireRate;

        UpdateUI();

        RaycastHit hit;

        if (Physics.Raycast(virtualCamera.transform.position, virtualCamera.transform.forward, out hit, stats.range))
        {
            Debug.Log($"Влучив у: {hit.collider.gameObject.name}");

            if (hit.collider.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
            {
                playerStats.TakeDamage(stats.damage);
                Debug.Log($"Нанесено шкоди гравцю. -{stats.damage} HP.");
            }
            else if (hit.collider.TryGetComponent<NPCStats>(out NPCStats npcStats))
            {
                npcStats.TakeDamage(stats.damage);
                Debug.Log($"Нанесено шкоди NPC. -{stats.damage} HP.");
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if (bulletTrailPrefab != null)
            {
                StartCoroutine(SpawnTrail(firePoint.position, hit.point, hit.collider.gameObject.GetComponent<Rigidbody>()));
            }
        }
        else
        {
            if (bulletTrailPrefab != null)
            {
                StartCoroutine(SpawnTrail(firePoint.position, virtualCamera.transform.position + virtualCamera.transform.forward * stats.range, null));
            }
        }
    }

    public void Reload()
    {
        if (!isReloading && currentAmmoInMagazine < stats.magazineSize && totalAmmo > 0)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Перезарядка...");
        yield return new WaitForSeconds(stats.reloadTime);
        int bulletsToReload = (int)Mathf.Min(stats.magazineSize - currentAmmoInMagazine, totalAmmo);
        currentAmmoInMagazine += bulletsToReload;
        totalAmmo -= bulletsToReload;

        isReloading = false;
        UpdateUI();
        Debug.Log("Перезаряджено!");
    }

    public IEnumerator SpawnTrail(Vector3 start, Vector3 end, Rigidbody rb)
    {
        GameObject trailGO = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        TrailRenderer trail = trailGO.GetComponent<TrailRenderer>();

        float speed = 200f;
        float distance = Vector3.Distance(start, end);
        float travelTime = distance / speed;

        Vector3 direction = (end - start).normalized + new Vector3(
            Random.Range(-stats.spread, stats.spread),
            Random.Range(-stats.spread, stats.spread),
            Random.Range(-stats.spread, stats.spread)
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
            Debug.LogError("GunFire: 'Ammotext' (TextMeshProUGUI) not found!");

        if (Player == null)
            Debug.LogError("GunFire: 'Player' not found!");
    }
}