using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using TMPro;

/// <summary>
/// This script handles showing the tooltips of each button when they are in focus
/// </summary>
public class ToolTipScript : MonoBehaviour
{
    [Tooltip("The TextMeshPro to display the tool tips on.")]
    public TextMeshPro subtext;

    /// <summary>
    /// Set the subtext to the default value.
    /// </summary>
    void Reset()
    {
        subtext.text = "Say 'Help' for assistance.";
    }

    /// <summary>
    /// Called once per frame. Resets the text and calls SetMenUSubText in case the button in focus has changed.
    /// </summary>
    void Update()
    {
        Reset();
        SetMenuSubText();
    }

    /// <summary>
    /// Sets the submenu text based on which of the four buttons are in focus, if any. 
    /// </summary>
    public void SetMenuSubText()
    {
        if (GazeManager.Instance.HitObject!= null)
        {
            if (GazeManager.Instance.HitObject.name == "LabelButton")
            {
                subtext.text = "Use a Label Tag to label certain parts and components and leave a comment on it. For Example - 'Valve 69 carrying gas to primary well head' ";
            }

            else if (GazeManager.Instance.HitObject.name == "HistoryButton")
            {
                subtext.text = "Use a History Tag to mark components which have a notorious history or notable facts. For Example - 'Pipe 420 fixed minor leak as of February 10. Check bottom for corrosion' ";
            }

            else if (GazeManager.Instance.HitObject.name == "FaultButton")
            {
                subtext.text = "Use a Fault Tag to mark components with active leaks or faults that require fixing. For Example - 'Tap 666 moderate leak of 800 ppm' ";
            }

            else if (GazeManager.Instance.HitObject.name == "BoundingButton")
            {
                subtext.text = "Use a Bounding Box to mark a general area of the location of a fault.";
            }
        }
    }
}