using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool shopOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ShopManager.Instance != null)
        {
            Debug.Log("������� ������ � ���� ��������. �������� ������ �����䳿.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && ShopManager.Instance != null && !shopOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ShopManager.Instance.OpenShop();
                shopOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && ShopManager.Instance != null)
        {
            ShopManager.Instance.CloseShop();
            shopOpen = false;
        }
    }
}