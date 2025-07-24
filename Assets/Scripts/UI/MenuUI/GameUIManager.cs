using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public GameObject keybindsPanel;

    [Header("In-Game HUD Elements")]
    public List<GameObject> inGameHudElements = new List<GameObject>();

    [Header("Player Control Scripts")]
    public PlayerMovement playerMovementScript;
    public GunFire gunFireScript;

    private List<GameObject> managerUIPanels = new List<GameObject>();

    public bool AnyManagerUIPanelActive
    {
        get
        {
            foreach (GameObject panel in managerUIPanels)
            {
                if (panel != null && panel.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }

    void Awake()
    {
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
        if (pauseMenuPanel != null) managerUIPanels.Add(pauseMenuPanel);
        if (settingsPanel != null) managerUIPanels.Add(settingsPanel);
        if (keybindsPanel != null) managerUIPanels.Add(keybindsPanel);

        foreach (GameObject panel in managerUIPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        SetGameUIAndPlayerControlActive(true);
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
        Application.Quit();
    }

    // ����������� ����� ���� �����
    public void TogglePauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            bool wasActive = pauseMenuPanel.activeSelf;

            foreach (GameObject panel in managerUIPanels)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }

            if (!wasActive)
            {
                pauseMenuPanel.SetActive(true);
                SetGameUIAndPlayerControlActive(false);
            }
            else
            {
                SetGameUIAndPlayerControlActive(true);
            }

            UpdateCursorState();
        }
    }

    // ³������� ����� �����������
    public void OpenSettingsPanel()
    {
        if (settingsPanel != null)
        {
            foreach (GameObject panel in managerUIPanels)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
            settingsPanel.SetActive(true);
            SetGameUIAndPlayerControlActive(false);
            UpdateCursorState();
        }
    }

    // �������� ����� �����������
    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(true);
                // ���� ���� �������� ����������� ������� ���� �����, HUD ���������� ����������
                // ��������� ������� ����� ���������� ������������
            }
            else
            {
                SetGameUIAndPlayerControlActive(true);
            }
            UpdateCursorState();
        }
    }

    // ³������� ����� ����������� �����
    public void OpenKeybindsPanel()
    {
        if (keybindsPanel != null)
        {
            foreach (GameObject panel in managerUIPanels)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
            keybindsPanel.SetActive(true);
            SetGameUIAndPlayerControlActive(false);
            UpdateCursorState();
        }
    }

    // �������� ����� ����������� �����
    public void CloseKeybindsPanel()
    {
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(false);
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
            else
            {
                SetGameUIAndPlayerControlActive(true);
            }
            UpdateCursorState();
        }
    }

    // ��������� ����� ��� �������� �������� HUD �� ��������� ������� ������
    private void SetGameUIAndPlayerControlActive(bool active)
    {
        foreach (GameObject hudElement in inGameHudElements)
        {
            if (hudElement != null)
            {
                hudElement.SetActive(active);
            }
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = active;
        }
        if (gunFireScript != null)
        {
            gunFireScript.enabled = active;
        }

        Debug.Log($"Game UI and Player Control set to: {active}");
    }

    void UpdateCursorState()
    {
        // ������ ������������� � �������, ���� ����-��� � ��������� UI ������� �������
        // ��� ���� KeybindManager �������� � ����� ��������������� (������ ����� �� ���� �������)
        if (AnyManagerUIPanelActive || (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding))
        {
            SetCursorLocked(false);
            SetCursorVisible(true);
        }
        else // ������, ������ ������������ � ����������
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
