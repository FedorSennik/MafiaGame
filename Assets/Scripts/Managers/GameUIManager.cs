using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    }

    void Start()
    {
        if (pauseMenuPanel != null) managerUIPanels.Add(pauseMenuPanel);
        if (settingsPanel != null) managerUIPanels.Add(settingsPanel);
        if (keybindsPanel != null) managerUIPanels.Add(keybindsPanel);
        if (shopPanel != null) managerUIPanels.Add(shopPanel);

        foreach (GameObject panel in managerUIPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        SetGameUIAndPlayerControlActive(true);
        UpdateCursorState();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleMenuKey))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(toggleShopKey))
        {
            ToggleShop();
        }

        UpdateCursorState();
    }

    public void SetGameUIAndPlayerControlActive(bool active)
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
    }

    private void UpdateCursorState()
    {
        if (AnyManagerUIPanelActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuPanel == null) return;

        bool isMenuOpen = pauseMenuPanel.activeSelf;
        pauseMenuPanel.SetActive(!isMenuOpen);

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (keybindsPanel != null) keybindsPanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);

        SetGameUIAndPlayerControlActive(!pauseMenuPanel.activeSelf);
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
