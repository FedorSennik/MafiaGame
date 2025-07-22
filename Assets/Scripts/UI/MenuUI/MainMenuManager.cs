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

    // Функція, яка викликається кнопкою "Грати"
    public void StartGame()
    {
        Debug.Log("Завантаження сцени гри...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene"); // Завантажуємо сцену "GameScene"
    }

    // Функція, яка викликається кнопкою "Налаштування" в головному меню
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
        Debug.Log("Відкрито Налаштування в головному меню.");
    }

    // Функція, яка викликається кнопкою "Назад" у налаштуваннях головного меню
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
        Debug.Log("Закрито Налаштування. Повернуто до Головного меню.");
    }

    // Функція, яка викликається кнопкою "Налаштування Клавіш" з панелі налаштувань головного меню
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
        Debug.Log("Відкрито Налаштування Клавіш в головному меню.");
    }

    // Функція, яка викликається кнопкою "Назад" з панелі налаштувань клавіш головного меню
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
        Debug.Log("Закрито Налаштування Клавіш. Повернуто до Налаштувань.");
    }

    // Функція, яка викликається кнопкою "Вихід"
    public void QuitGame()
    {
        Debug.Log("Вихід з гри...");
        Application.Quit();
    }
}
