using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// This script handles the control and inputs of the HelpBox
/// </summary>
public class HelpBoxScript : MonoBehaviour, IInputClickHandler
{
    [Tooltip("The TextMeshPro of the help box.")]
    public TextMeshPro textMeshPro;
    private int pageCount; //The current number of pages.
    private int currentPage; //The page currently in view. Counts starting at 1. 

    /// <summary>
    /// Initializes pageCount and CurrentPage
    /// </summary>
    void Awake()
    {
        pageCount = textMeshPro.textInfo.pageCount;
        currentPage = textMeshPro.pageToDisplay;
    }

    /// <summary>
    /// When the help box is enabled, sets the current page to page 1.
    /// </summary>
    void OnEnable()
    {
        currentPage=1;
        textMeshPro.pageToDisplay = currentPage;
    }


    /// <summary>
    /// When the help box is tapped, this turns to the next page.
    /// </summary>
    /// <param name="eventData">Event data for the input.</param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("tap recognised");
        turnPageForward();
    }

    /// <summary>
    /// Increments the current displayed page in the text mesh if there are more pages left.
    /// </summary>
    public void turnPageForward()
    {
        if (pageCount > currentPage)
        {
            currentPage++;
            textMeshPro.pageToDisplay = currentPage;           
        }
    }

    /// <summary>
    /// Decrements the current displayed page in the text mesh if there are prior pages.
    /// </summary>
    public void turnPageBackward()
    {
        Debug.Log("you said back");
        if (currentPage > 1)
        {
            currentPage--;
            textMeshPro.pageToDisplay = currentPage;
        }
    }

}
