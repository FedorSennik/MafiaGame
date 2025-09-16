using UnityEngine;

public class Crime : MonoBehaviour
{
    [Header("������������ �������")]
    public int wantedLevelIncrease = 1;
    public bool destroyAfterUse = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (WantedLevel.Instance != null)
            {
                WantedLevel.Instance.IncreaseWantedLevel(wantedLevelIncrease);
                Debug.Log("������ �����!");
                if (destroyAfterUse)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("WantedLevel.Instance �� ��������. �������������, �� ������ WantedLevel ����������� �� ������.");
            }
        }
    }
}