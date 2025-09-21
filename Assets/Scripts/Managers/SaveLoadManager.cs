using UnityEngine;
using System.IO;
using System.Text;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    private string saveFilePath;
    private const string SAVE_FILE_NAME = "savedata.json";

    [Header("Налаштування автозбереження")]
    public bool autoSaveEnabled = true;
    public float autoSaveInterval = 180f;
    private float autoSaveTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            autoSaveTimer = autoSaveInterval;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (autoSaveEnabled)
        {
            autoSaveTimer -= Time.deltaTime;
            if (autoSaveTimer <= 0)
            {
                SaveGame();
                autoSaveTimer = autoSaveInterval;
            }
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Гра завершується. Виконується автоматичне збереження...");
        SaveGame();
    }

    public void SaveGame()
    {
        if (WantedLevel.Instance == null || PlayerStats.Instance == null)
        {
            Debug.LogError("SaveLoadManager: Неможливо зберегти гру, оскільки не знайдено один з менеджерів.");
            return;
        }

        GameData data = new GameData();

        data.playerPosition = PlayerStats.Instance.transform.position;
        data.playerMoney = PlayerStats.Instance.PlayerMoney;
        data.stealMoney = PlayerStats.Instance.StealMoney;
        data.wantedLevel = WantedLevel.Instance.wantedLevel;

        string json = JsonUtility.ToJson(data);

        try
        {
            File.WriteAllText(saveFilePath, json, Encoding.UTF8);
            Debug.Log("Гра успішно збережена!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Помилка при збереженні гри: {e.Message}");
        }
    }

    public void LoadGame()
    {
        if (WantedLevel.Instance == null || PlayerStats.Instance == null)
        {
            Debug.LogError("SaveLoadManager: Неможливо завантажити гру, оскільки не знайдено один з менеджерів.");
            return;
        }

        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath, Encoding.UTF8);

                GameData data = JsonUtility.FromJson<GameData>(json);

                PlayerStats.Instance.transform.position = data.playerPosition;
                PlayerStats.Instance.PlayerMoney = data.playerMoney;
                PlayerStats.Instance.StealMoney = data.stealMoney;
                WantedLevel.Instance.wantedLevel = data.wantedLevel;

                PlayerStats.Instance.UpdateUI();

                Debug.Log("Гра успішно завантажена!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Помилка при завантаженні гри: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Файл збереження не знайдено!");
        }
    }
}