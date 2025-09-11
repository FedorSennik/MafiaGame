using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyLaundering : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI launderingText;

    private bool canLaunder = false;

    private void OnCollisionEnter(Collision collision)
    {
        launderingText.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canLaunder = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        launderingText.gameObject.SetActive(false);
        Cursor.visible = true;
        canLaunder = false;
    }

    private void Update()
    {
        if (canLaunder && Input.GetKeyDown(KeyCode.F))
        {
            LaunderMoney();
        }
    }

    public void LaunderMoney()
    {
        PlayerStats.Instance.PlayerMoney += PlayerStats.Instance.StealMoney * PlayerStats.Instance.DealerTrusts;
        PlayerStats.Instance.StealMoney = 0f;
        PlayerStats.Instance.UpdateUI();
    }
}