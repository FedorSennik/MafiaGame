using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (WantedLevel.Instance != null)
            {
                WantedLevel.Instance.isPlayerInside = true;
                Debug.Log("Гравець увійшов до будівлі.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (WantedLevel.Instance != null)
            {
                WantedLevel.Instance.isPlayerInside = false;
                Debug.Log("Гравець покинув будівлю.");
            }
        }
    }
}