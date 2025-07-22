using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    // Singleton-��������� ��� ������� ������� � ����� �������
    public static GameUIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public GameObject keybindsPanel;

    private List<GameObject> allUIPanels = new List<GameObject>();

    // ����������, ��� �����, �� ������ �� UI �����
    public bool AllUIIsClosed
    {
        get
        {
            foreach (GameObject panel in allUIPanels)
            {
                if (panel != null && panel.activeSelf)
                {
                    return false;
                }
            }
            if (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding)
            {
                return false;
            }
            return true;
        }
    }

    void Awake()
    {
        // ��������� Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pauseMenuPanel != null) allUIPanels.Add(pauseMenuPanel);
        if (settingsPanel != null) allUIPanels.Add(settingsPanel);
        if (keybindsPanel != null) allUIPanels.Add(keybindsPanel);

        foreach (GameObject panel in allUIPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        UpdateCursorState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        UpdateCursorState();
    }

    // �������, ��� ����������� ������� "������� ����" � ��
    public void BackToMainMenu()
    {
        Debug.Log("���������� �� ��������� ����...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); // ����������� ����� "MainMenuScene"
    }

    // �������, ��� ����������� ������� "�����" � ���
    public void QuitGame()
    {
        Debug.Log("����� � ���...");
        Application.Quit(); // �� ������� ������ ����� � ������ ��
    }

    // ����������� ����� ���� �����
    public void TogglePauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            bool isActive = pauseMenuPanel.activeSelf;
            pauseMenuPanel.SetActive(!isActive);

            if (!isActive)
            {
                if (settingsPanel != null) settingsPanel.SetActive(false);
                if (keybindsPanel != null) keybindsPanel.SetActive(false);
            }

            UpdateCursorState();
        }
    }

    // ³������� ����� �����������
    public void OpenSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (keybindsPanel != null) keybindsPanel.SetActive(false);
            UpdateCursorState();
        }
    }

    // �������� ����� �����������
    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
            UpdateCursorState();
        }
    }

    // ³������� ����� ����������� �����
    public void OpenKeybindsPanel()
    {
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(true);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            UpdateCursorState();
        }
    }

    // �������� ����� ����������� �����
    public void CloseKeybindsPanel()
    {
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);
            UpdateCursorState();
        }
    }

    // ��������� ����� ������� (�������/����������, ������������/�������������)
    void UpdateCursorState()
    {
        if (!AllUIIsClosed || (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding))
        {
            SetCursorLocked(false);
            SetCursorVisible(true);
        }
        else
        {
            SetCursorLocked(true);
            SetCursorVisible(false);
        }
    }

    // ������� ������ ��� ��������� ��������
    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }
}
