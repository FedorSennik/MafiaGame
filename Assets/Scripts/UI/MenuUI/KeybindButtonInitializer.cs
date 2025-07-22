using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeybindButtonInitializer : MonoBehaviour
{
    public string actionName;
    public TextMeshProUGUI keyTextElement;

    void Start()
    {
        if (KeybindManager.Instance == null)
        {
            Debug.LogError("KeybindManager.Instance не знайдено! Переконайтеся, що він присутній на сцені.");
            return;
        }

        KeybindManager.Instance.RegisterKeybindText(actionName, keyTextElement);

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => KeybindManager.Instance.StartRebind(actionName));
        }
        else
        {
            Debug.LogError($"Кнопка не знайдена на об'єкті {gameObject.name}!");
        }
    }
}
