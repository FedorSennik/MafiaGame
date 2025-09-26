using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public GameObject keybindsPanel;
    public GameObject shopPanel;

    [Header("Player Control Scripts")]
    public PlayerMovement playerMovementScript;
    public GunFire gunFireScript;

    [Header("In-Game HUD Elements")]
    public List<GameObject> inGameHudElements = new List<GameObject>();

    [Header("Keybinds")]
    public KeyCode toggleMenuKey = KeyCode.Escape;
    public KeyCode toggleShopKey = KeyCode.B;

    private List<GameObject> managerUIPanels = new List<GameObject>();

    public bool AnyManagerUIPanelActive
    {
        get
        {
            bool uiPanelActive = managerUIPanels.Any(panel => panel != null && panel.activeSelf);
            bool isRebinding = KeybindManager.Instance != null && KeybindManager.Instance.isRebinding;
            return uiPanelActive || isRebinding;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        managerUIPanels.Add(pauseMenuPanel);
        managerUIPanels.Add(settingsPanel);
        managerUIPanels.Add(keybindsPanel);
        managerUIPanels.Add(shopPanel);
    }

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (keybindsPanel != null) keybindsPanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleMenuKey))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(toggleShopKey) && !pauseMenuPanel.activeSelf && !settingsPanel.activeSelf && !keybindsPanel.activeSelf)
        {
            ToggleShop();
        }

        UpdateInGameHUDVisibility();
    }

    private void SetGameUIAndPlayerControlActive(bool isActive)
    {
        Time.timeScale = isActive ? 1f : 0f;

        if (playerMovementScript != null) playerMovementScript.enabled = isActive;
        if (gunFireScript != null) gunFireScript.enabled = isActive;

    }

    private void UpdateInGameHUDVisibility()
    {
        bool isAnyPanelActive = AnyManagerUIPanelActive;

        foreach (var hudElement in inGameHudElements)
        {
            if (hudElement != null)
            {
                hudElement.SetActive(!isAnyPanelActive);
            }
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuPanel == null) return;

        if (settingsPanel != null && settingsPanel.activeSelf) CloseSettingsPanel();
        if (keybindsPanel != null && keybindsPanel.activeSelf) CloseKeybindsPanel();
        if (shopPanel != null && shopPanel.activeSelf) ToggleShop();


        bool isPaused = pauseMenuPanel.activeSelf;
        pauseMenuPanel.SetActive(!isPaused);
        SetGameUIAndPlayerControlActive(isPaused);

        if (pauseMenuPanel.activeSelf)
        {
            Debug.Log("Гра призупинена.");
        }
        else
        {
            Debug.Log("Гра відновлена.");
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel == null) return;

        if (pauseMenuPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(false);
            SetGameUIAndPlayerControlActive(true);
            Debug.Log("Гра відновлена (кнопка).");
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettingsPanel()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (keybindsPanel != null) keybindsPanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);

        SetGameUIAndPlayerControlActive(false);
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

        if (!AnyManagerUIPanelActive)
        {
            SetGameUIAndPlayerControlActive(true);
        }
    }

    public void OpenKeybindsPanel()
    {
        if (keybindsPanel == null) return;

        keybindsPanel.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);

        SetGameUIAndPlayerControlActive(false);
    }

    public void CloseKeybindsPanel()
    {
        if (keybindsPanel == null) return;

        keybindsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        if (!AnyManagerUIPanelActive)
        {
            SetGameUIAndPlayerControlActive(true);
        }
    }

    public void ToggleShop()
    {
        if (shopPanel == null) return;

        bool isShopOpen = shopPanel.activeSelf;
        shopPanel.SetActive(!isShopOpen);

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (keybindsPanel != null) keybindsPanel.SetActive(false);

        SetGameUIAndPlayerControlActive(!shopPanel.activeSelf);

        if (shopPanel.activeSelf && ShopManager.Instance != null)
        {
            ShopManager.Instance.OpenShop();
        }
        else if (!shopPanel.activeSelf && ShopManager.Instance != null)
        {
            ShopManager.Instance.CloseShop();
        }
    }
}