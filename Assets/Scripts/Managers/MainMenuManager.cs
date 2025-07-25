using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject keybindsPanel;

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
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // �������, ��� ����������� ������� "�����"
    public void StartGame()
    {
        Debug.Log("������������ ����� ���...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene"); // ����������� ����� "GameScene"
    }

    // �������, ��� ����������� ������� "������������" � ��������� ����
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
        Debug.Log("³������ ������������ � ��������� ����.");
    }

    // �������, ��� ����������� ������� "�����" � ������������� ��������� ����
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
        Debug.Log("������� ������������. ��������� �� ��������� ����.");
    }

    // �������, ��� ����������� ������� "������������ �����" � ����� ����������� ��������� ����
    public void OpenKeybinds()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(true);
        }
        Debug.Log("³������ ������������ ����� � ��������� ����.");
    }

    // �������, ��� ����������� ������� "�����" � ����� ����������� ����� ��������� ����
    public void CloseKeybinds()
    {
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        Debug.Log("������� ������������ �����. ��������� �� �����������.");
    }

    // �������, ��� ����������� ������� "�����"
    public void QuitGame()
    {
        Debug.Log("����� � ���...");
        Application.Quit();
    }
}
