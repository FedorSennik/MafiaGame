using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

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
    private PlayerStats playerStats;

    [Header("GameObject")]
    public GameObject hitEffect;
    public GameObject bulletTrailPrefab;
    public GameObject Player;

    public KeyCode reloadKey = KeyCode.R;

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
    }

    void Update()
    {
        if (isReloading) return;

        if (currentAmmoInMagazine <= 0 || Input.GetKeyDown(reloadKey)&& isEquiped)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && isEquiped && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + stats.fireRate;
            Shoot();
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

            StartCoroutine(SpawnTracer(firePoint.position, hitPoint, stats.spread));
        
    }

    IEnumerator SpawnTracer(Vector3 start, Vector3 end, float spread)
    {
        GameObject trailGO = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        TrailRenderer trail = trailGO.GetComponent<TrailRenderer>();

        float speed = 200f;
        float distance = Vector3.Distance(start, end);
        float travelTime = distance / speed;

        // ▶️ Добавляем разброс (spread)
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

    public void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmoInMagazine} / {totalAmmo}";
    }

    public void Finder()
    {
            GameObject camObj = GameObject.Find("MainCamera");
            virtualCamera = camObj.GetComponent<Camera>();
            GameObject textObj = GameObject.Find("Ammotext");
            ammoText = textObj.GetComponent<TextMeshProUGUI>();
            Player = GameObject.Find("Player");
            Player.GetComponent<PlayerStats>();
      
    }
}