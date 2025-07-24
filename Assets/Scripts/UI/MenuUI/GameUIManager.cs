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

    // Функція, яка викликається кнопкою "Головне меню" в грі
    public void BackToMainMenu()
    {
        Debug.Log("Повернення до головного меню...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); // Завантажуємо сцену "MainMenuScene"
    }

    // Функція, яка викликається кнопкою "Вихід" з гри
    public void QuitGame()
    {
        Debug.Log("Вихід з гри...");
        Application.Quit();
    }

    // Перемикання стану меню паузи
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

    // Відкриття панелі налаштувань
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

    // Закриття панелі налаштувань
    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(true);
                // Якщо після закриття налаштувань відкрите меню паузи, HUD залишається прихованим
                // Управління гравцем також залишається заблокованим
            }
            else
            {
                SetGameUIAndPlayerControlActive(true);
            }
            UpdateCursorState();
        }
    }

    // Відкриття панелі налаштувань клавіш
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

    // Закриття панелі налаштувань клавіш
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

    // Допоміжний метод для контролю видимості HUD та активності скриптів гравця
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
        // Курсор розблокований і видимий, якщо будь-яка з КЕРОВАНИХ UI панелей активна
        // АБО якщо KeybindManager перебуває в режимі перепризначення (курсор також має бути видимим)
        if (AnyManagerUIPanelActive || (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding))
        {
            SetCursorLocked(false);
            SetCursorVisible(true);
        }
        else // Інакше, курсор заблокований і прихований
        {
            SetCursorLocked(true);
            SetCursorVisible(false);
        }
    }

    // Допоміжні методи для керування курсором
    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }
}
