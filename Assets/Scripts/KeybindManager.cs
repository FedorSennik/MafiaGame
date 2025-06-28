using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class KeybindManager : MonoBehaviour
{
   
    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    public string currentBindingKey;
    public bool isRebinding = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isRebinding)
        {
            ChangeKeyBinding();
        }
    }

    public void Init()
    {
        keyBindings.Add("Reload", KeyCode.R);
        keyBindings.Add("Shoot", KeyCode.Mouse0);
        keyBindings.Add("Jump", KeyCode.Space);
        keyBindings.Add("Sprint", KeyCode.LeftShift);
    }

    public void Rebind(string bindingKey)
    {
        currentBindingKey = bindingKey;
        isRebinding = true;
    }

    public void ChangeKeyBinding()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                keyBindings[currentBindingKey] = key;
                GameObject.Find(currentBindingKey + "Text").GetComponent<TextMeshProUGUI>().text=key.ToString();
                isRebinding = false;
                break;

            }
        }
    }

}