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

    // Функція, яка викликається кнопкою "Грати" або "Головне меню"
    public void StartGame()
    {
        Debug.Log("Завантаження сцени гри...");
        // Завантажуємо сцену "GameScene" за її назвою
        SceneManager.LoadScene("GameScene");
    }

    // Функція, яка викликається кнопкою "Налаштування"
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
        Debug.Log("Відкрито Налаштування.");
    }

    // Функція, яка викликається кнопкою "Назад" у налаштуваннях
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Приховуємо панель налаштувань
        }
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // Показуємо головне меню
        }
        Debug.Log("Закрито Налаштування. Повернуто до Головного меню.");
    }

    // Функція, яка викликається кнопкою "Вихід"
    public void QuitGame()
    {
        Debug.Log("Вихід з гри...");
        // Ця функція працює тільки в зібраній грі (exe, apk тощо).
        // У редакторі Unity вона просто зупиняє виконання.
        Application.Quit();
    }
}
