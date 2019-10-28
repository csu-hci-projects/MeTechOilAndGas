using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDevConsoleScript : MonoBehaviour
{
    /// <summary>
    /// Simple script to close the dev console Unity's weird error opens by default
    /// </summary>
    void Start()
    {
        Debug.developerConsoleVisible = false;
    }
}
