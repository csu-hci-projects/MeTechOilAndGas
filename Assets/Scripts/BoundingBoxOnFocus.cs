using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script is used to connect each Bounding Box object to a TagManager.
/// When the BB is in focus, it sets the tagInFocus var in TagManager to the BB.
/// This allows for deletion of the BB in focus.
/// Inherits from IFocusable for OnFocusEnter and OnFocusExit methods. 
/// </summary>
public class BoundingBoxOnFocus : MonoBehaviour, IFocusable
{

    private TagManager tagManager;

    /// <summary>
    /// At startup, once the TagManager is awake get it's singleton instance reference
    /// </summary>
    void Start()
    {
        tagManager = TagManager.instance;
    }

    /// <summary>
    /// When the Bounding Box enters focus, set itself as the tagInFocus
    /// </summary>
    public void OnFocusEnter()
    {
        tagManager.SetTagInFocus(this.gameObject);
    }

    /// <summary>
    /// When the Bounding Box is no longer in focus, remove itself from the TagManager's tagInFocus
    /// </summary>
    public void OnFocusExit()
    {
        if (tagManager.GetTagInFocus() == this.gameObject)
        {
            tagManager.SetTagInFocus(null);
        }
    }
}
