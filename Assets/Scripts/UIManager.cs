using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;
    [SerializeField]private bool _allUIIsClosed;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (!Menu.activeSelf && !Settings.activeSelf)
        {
            _allUIIsClosed = true;
        }
        else { _allUIIsClosed= false; }

    }

    public bool AllUIIsClosed
    {
        get { return _allUIIsClosed; }
        set { _allUIIsClosed = value; }
    }
    private void Start()
    {
        _allUIIsClosed = true;
    }

    
    public void ToggleMenu()
    {
        bool menuIsActive = Menu.activeSelf;
        Menu.SetActive(!menuIsActive);
        Settings.SetActive(false);
    }

    public void ToggleSettings()
    {
        Settings.SetActive(true);
        Menu.SetActive(false);
    }
}
