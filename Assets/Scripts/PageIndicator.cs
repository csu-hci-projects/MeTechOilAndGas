using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script to handle displaying pages, attach to a TextMeshPro
/// </summary>
public class PageIndicator : MonoBehaviour
{

    private TextMeshPro text; //The TextMeshPro of the gameObject this is attached to

    /// <summary>
    /// Get the TextMeshPro
    /// </summary>
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshPro>();        
    }
    
    /// <summary>
    /// When this is called, update the TextMeshPro to display the new Page number
    /// </summary>
    /// <param name="pageNo">The new Page Number to display</param>
    public void UpdatePage(int pageNo)
    {       
        text.text = "Page " + pageNo;        
    }
}