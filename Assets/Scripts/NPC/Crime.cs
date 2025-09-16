using UnityEngine;

public class Crime : MonoBehaviour
{
    [Header("Налаштування злочину")]
    public int wantedLevelIncrease = 1;
    public bool destroyAfterUse = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (WantedLevel.Instance != null)
            {
                WantedLevel.Instance.IncreaseWantedLevel(wantedLevelIncrease);
                Debug.Log("Злочин скоєно!");
                if (destroyAfterUse)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("WantedLevel.Instance не знайдено. Переконайтеся, що скрипт WantedLevel прикріплений до гравця.");
            }
        }
    }
}