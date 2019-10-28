using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles the visibility and active state toggling of the menu and the help box.
/// </summary>
public class ToggleMenuScript : MonoBehaviour
{
    [Tooltip("The GameObject to be used as the menu.")]
    public GameObject menu;
    [Tooltip("The GameObject to be used as the help box.")]
    public GameObject help;

    public static ToggleMenuScript instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        help.SetActive(true);
        menu.SetActive(false);
    }

    /// <summary>
    /// Hides the menu from view.
    /// </summary>
    public void HideMenu()
    {
        Debug.Log("HideMenu Called");
        menu.SetActive(false);
    }

    /// <summary>
    /// Deactivates the help box if it is open and activates the menu.
    /// </summary>
    public void ShowMenu()
    {
        if (help.activeSelf)
            help.SetActive(false);
        menu.SetActive(true);
        Debug.Log("ShowMenu Called"+menu.activeSelf);
    }

    /// <summary>
    /// Deactivates the menu if active and activates the help box.
    /// </summary>
    public void ShowHelp()
    {
        Debug.Log("ShowHelp Called");
        if (menu.activeSelf)
            menu.SetActive(false);
        help.SetActive(true);
    }

    /// <summary>
    /// Deactivates and hides the help box from view.
    /// </summary>
    public void CloseHelp()
    {
        Debug.Log("CloseHelp Called");
        help.SetActive(false);
    }
}