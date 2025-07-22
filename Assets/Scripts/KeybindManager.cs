using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance { get; private set; }

    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();

    public Dictionary<string, TextMeshProUGUI> keybindTexts = new Dictionary<string, TextMeshProUGUI>();

    private string currentBindingKey;
    public bool isRebinding = false;

    void Awake()
    {
        // ��������� Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitDefaultKeyBindings();
        LoadKeyBindings();
    }

    void Update()
    {
        if (isRebinding)
        {
            ChangeKeyBinding();
        }
    }

    // ����������� ����'���� ����� �� �������������
    void InitDefaultKeyBindings()
    {
        keyBindings.Clear();

        keyBindings.Add("Forward", KeyCode.W);
        keyBindings.Add("Backward", KeyCode.S);
        keyBindings.Add("Left", KeyCode.A);
        keyBindings.Add("Right", KeyCode.D);
        keyBindings.Add("Jump", KeyCode.Space);
        keyBindings.Add("Shoot", KeyCode.Mouse0); // ˳�� ������ ����
        keyBindings.Add("Scope", KeyCode.Mouse1); // ����� ������ ����
        keyBindings.Add("Reload", KeyCode.R);
        keyBindings.Add("Sprint", KeyCode.LeftShift);
        // ������� ���� 䳿 �� ��������
    }

    public void StartRebind(string bindingKey)
    {
        currentBindingKey = bindingKey;
        isRebinding = true;
        Debug.Log($"���������� ���������� ������ ��� '{bindingKey}'...");

        if (keybindTexts.ContainsKey(bindingKey))
        {
            keybindTexts[bindingKey].text = "...";
        }
    }

    void ChangeKeyBinding()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                if (keyBindings.ContainsValue(key) && keyBindings[currentBindingKey] != key)
                {
                    string existingBinding = "";
                    foreach (var entry in keyBindings)
                    {
                        if (entry.Value == key)
                        {
                            existingBinding = entry.Key;
                            break;
                        }
                    }

                    KeyCode oldKey = keyBindings[currentBindingKey];
                    keyBindings[existingBinding] = oldKey;
                    if (keybindTexts.ContainsKey(existingBinding))
                    {
                        keybindTexts[existingBinding].text = oldKey.ToString();
                    }
                    Debug.Log($"������ '{key}' ��� ��������������� ��� '{existingBinding}'. �������������� '{existingBinding}' �� '{oldKey}'.");
                }

                keyBindings[currentBindingKey] = key;
                Debug.Log($"���������� '{key}' ��� 䳿 '{currentBindingKey}'.");

                if (keybindTexts.ContainsKey(currentBindingKey))
                {
                    keybindTexts[currentBindingKey].text = key.ToString();
                }

                isRebinding = false;
                SaveKeyBindings();

                break;
            }
        }
    }

    public KeyCode GetKey(string actionName)
    {
        if (keyBindings.ContainsKey(actionName))
        {
            return keyBindings[actionName];
        }
        Debug.LogWarning($"����'���� ��� 䳿 '{actionName}' �� ��������. ���������� �� �������������.");
        return KeyCode.None;
    }

    void SaveKeyBindings()
    {
        foreach (var entry in keyBindings)
        {
            PlayerPrefs.SetString("Keybind_" + entry.Key, entry.Value.ToString());
        }
        PlayerPrefs.Save();
        Debug.Log("����'���� ����� ���������.");
    }

    void LoadKeyBindings()
    {
        foreach (var entry in keyBindings)
        {
            string savedKeyString = PlayerPrefs.GetString("Keybind_" + entry.Key, entry.Value.ToString());
            KeyCode savedKeyCode;

            if (System.Enum.TryParse(savedKeyString, out savedKeyCode))
            {
                keyBindings[entry.Key] = savedKeyCode;
            }
            else
            {
                Debug.LogWarning($"�� ������� ���������� ��������� ������ '{savedKeyString}' ��� 䳿 '{entry.Key}'. ����������� �������� �� �������������.");
            }
        }
        Debug.Log("����'���� ����� �����������.");
        UpdateAllKeybindTexts();
    }

    public void RegisterKeybindText(string actionName, TextMeshProUGUI textElement)
    {
        if (!keybindTexts.ContainsKey(actionName))
        {
            keybindTexts.Add(actionName, textElement);
        }
        else
        {
            keybindTexts[actionName] = textElement;
        }
        if (keyBindings.ContainsKey(actionName))
        {
            textElement.text = keyBindings[actionName].ToString();
        }
    }

    void UpdateAllKeybindTexts()
    {
        foreach (var entry in keybindTexts)
        {
            if (keyBindings.ContainsKey(entry.Key))
            {
                entry.Value.text = keyBindings[entry.Key].ToString();
            }
        }
    }
}
