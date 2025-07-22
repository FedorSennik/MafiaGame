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

    // �������, ��� ����������� ������� "������� ����" � ��
    public void BackToMainMenu()
    {
        Debug.Log("���������� �� ��������� ����...");
        Time.timeScale = 1f;
        // ����������� ����� "MainMenuScene" �� �� ������
        SceneManager.LoadScene("MainMenuScene");
    }

    // �������, ��� ����������� ������� "�����" � ���
    public void QuitGame()
    {
        Debug.Log("����� � ���...");
        Application.Quit();
    }
}
