using UnityEngine;

public class PistolFire : MonoBehaviour
{
    [Header("Налаштування Пістолета")]
    public float damage = 15f;
    public float fireRate = 0.5f;
    public float range = 50f;

    private float nextTimeToFire = 0f;

    public void FireAtTarget(Transform shooterFirePoint, Transform target)
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;

            Vector3 fireDirection = (target.position - shooterFirePoint.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(shooterFirePoint.position, fireDirection, out hit, range))
            {
                PlayerStats playerStats = hit.transform.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(Mathf.RoundToInt(damage));
                }
            }
        }
    }
}