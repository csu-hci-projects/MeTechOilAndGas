using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A Manager to handle the page functions of the TagLabel's TextMeshPro.
/// </summary>
public class TextMeshProManager : MonoBehaviour
{

    private TextMeshPro textMeshPro; //Reference to the TextMeshPro
    private int pageCount; //Number of pages in the textMeshPro. 
    private int currentPage; //Reference to the current page number. Counts starting at 1
    [Tooltip("The Page indicator for this tag.")]
    public PageIndicator pageIndicator;

    /// <summary>
    /// Instantiate variables from the textMeshPro
    /// </summary>
    void Awake()
    {
        textMeshPro = this.gameObject.GetComponent<TextMeshPro>();
        pageCount = textMeshPro.textInfo.pageCount;
        currentPage = textMeshPro.pageToDisplay;
    }

    /// <summary>
    /// If at any time this object's page count differs from the TextMeshPro's
    /// Set the current page = page count current page is greater 
    /// And update the textMeshPro's current page accordingly. Update the PageIndicator as well.
    /// </summary>
    void Update()
    {
       
        if (pageCount!= textMeshPro.textInfo.pageCount)
        {
            pageCount = textMeshPro.textInfo.pageCount;
            if (currentPage > pageCount)
            {
                currentPage = pageCount;
                textMeshPro.pageToDisplay = currentPage;
                if (pageIndicator != null) { pageIndicator.UpdatePage(currentPage); }
            }
        }
        if (pageCount > 1)
        {
            pageIndicator.gameObject.SetActive(true);
            pageIndicator.UpdatePage(currentPage);
        }
        else pageIndicator.gameObject.SetActive(false);
    }

    /// <summary>
    /// If there are still pages to turn to, increase the current page by 1
    /// And update the textMeshPro and pageIndicator.
    /// </summary>
    public void turnPageForward()
    {
        Debug.Log("you said next");
        if (pageCount > currentPage)
        {
            currentPage++;
            textMeshPro.pageToDisplay = currentPage;
            if (pageIndicator != null) { pageIndicator.UpdatePage(currentPage); }
        }
    }

    /// <summary>
    /// If there are still pages to turn back to, decrease the current page by 1
    /// And update the textMeshPro and pageIndicator.
    /// </summary>
    public void turnPageBackward()
    {
        Debug.Log("you said back");
        if (currentPage > 1)
        {
            currentPage--;
            textMeshPro.pageToDisplay = currentPage;
            if (pageIndicator != null) { pageIndicator.UpdatePage(currentPage); }
        }
    }

    /// <summary>
    /// Get the current page that is being displayed, and set currentPage equal to that if not already
    /// </summary>
    /// <returns>Int with the current page number</returns>
    public int getCurrentPage()
    {
        if(currentPage!= textMeshPro.pageToDisplay) { currentPage = textMeshPro.pageToDisplay; }
        return currentPage;
    }

    /// <summary>
    /// Clears all text in the TextMeshPro.
    /// </summary>
    public void clearTag()
    {
        Debug.Log("you said clear");
        textMeshPro.text = " ";
    }

}
