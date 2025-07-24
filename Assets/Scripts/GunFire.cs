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

    public bool isEquiped;
    public bool isAdded;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        isAdded = false;
        Finder();
        isEquiped = false;
        totalAmmo = stats.maxAmmo;
        currentAmmoInMagazine = stats.magazineSize;
        UpdateUI();
        UpdateKeybinds();
    }

    void Update()
    {
        UpdateKeybinds();

        if (isReloading) return;

        if ((currentAmmoInMagazine <= 0 || Input.GetKeyDown(_reloadKey)) && isEquiped && currentAmmoInMagazine < stats.magazineSize && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKey(_shootKey) && isEquiped && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + stats.fireRate;
            Shoot();
        }
    }

    // Метод для оновлення приватних KeyCode змінних з KeybindManager
    void UpdateKeybinds()
    {
        if (KeybindManager.Instance != null)
        {
            _shootKey = KeybindManager.Instance.GetKey("Shoot");
            _reloadKey = KeybindManager.Instance.GetKey("Reload");
            //Debug.Log("GunFire: Keybinds updated from KeybindManager.");
        }
    }

    // Корутина для перезарядки зброї
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

    // Метод стрільби
    void Shoot()
    {
        Debug.Log("Shoot!");

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

        StartCoroutine(SpawnTracer(firePoint.position, hitPoint, stats.spread));
    }

    // Корутина для візуалізації трасера кулі з розкидом
    IEnumerator SpawnTracer(Vector3 start, Vector3 end, float spread)
    {
        GameObject trailGO = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        TrailRenderer trail = trailGO.GetComponent<TrailRenderer>();

        float speed = 200f;
        float distance = Vector3.Distance(start, end);
        float travelTime = distance / speed;

        Vector3 direction = (end - start).normalized;
        direction += new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );
        Vector3 finalEnd = start + direction * distance;

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

    // Оновлює текстовий елемент UI боєприпасів
    public void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmoInMagazine} / {totalAmmo}";
    }

    // Знаходить необхідні посилання на об'єкти на сцені
    public void Finder()
    {
        GameObject camObj = GameObject.Find("MainCamera");
        if (camObj != null)
        {
            virtualCamera = camObj.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("GunFire: Об'єкт 'MainCamera' не знайдено!");
        }

        GameObject textObj = GameObject.Find("Ammotext");
        if (textObj != null)
        {
            ammoText = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("GunFire: Об'єкт 'Ammotext' (TextMeshProUGUI) не знайдено. UI боєприпасів не буде відображатися.");
        }

        Player = GameObject.Find("Player");
        if (Player == null)
        {
            Debug.LogError("GunFire: Об'єкт гравця з тегом 'Player' не знайдено!");
        }
    }
}
