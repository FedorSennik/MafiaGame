using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Pause Menu Panel")]
    public GameObject MenuPanel;

    void Start()
    {
        if (MenuPanel != null)
        {
            MenuPanel.SetActive(false);
        }
    }

    // Функція, яка викликається кнопкою "Головне меню" в грі
    public void BackToMainMenu()
    {
        Debug.Log("Повернення до головного меню...");
        Time.timeScale = 1f;
        // Завантажуємо сцену "MainMenuScene" за її назвою
        SceneManager.LoadScene("MainMenuScene");
    }

    // Функція, яка викликається кнопкою "Вихід" з гри
    public void QuitGame()
    {
        Debug.Log("Вихід з гри...");
        Application.Quit();
    }
}
