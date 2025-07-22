using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    void Start()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // �������, ��� ����������� ������� "�����" ��� "������� ����"
    public void StartGame()
    {
        Debug.Log("������������ ����� ���...");
        // ����������� ����� "GameScene" �� �� ������
        SceneManager.LoadScene("GameScene");
    }

    // �������, ��� ����������� ������� "������������"
    public void OpenSettings()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        Debug.Log("³������ ������������.");
    }

    // �������, ��� ����������� ������� "�����" � �������������
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // ��������� ������ �����������
        }
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // �������� ������� ����
        }
        Debug.Log("������� ������������. ��������� �� ��������� ����.");
    }

    // �������, ��� ����������� ������� "�����"
    public void QuitGame()
    {
        Debug.Log("����� � ���...");
        // �� ������� ������ ����� � ������ �� (exe, apk ����).
        // � �������� Unity ���� ������ ������� ���������.
        Application.Quit();
    }
}
