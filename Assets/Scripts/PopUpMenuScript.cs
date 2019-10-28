using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using TMPro;

/// <summary>
/// This script handles the effect of "Popping Up" the tags while in focus. 
/// Handles swapping between active and inactive states while the tag is in or out of focus.
/// </summary>
public class PopUpMenuScript : MonoBehaviour, IFocusable
{
    [Tooltip("The GameObject representing the inactive state.")]
    public GameObject inactivePop;
    [Tooltip("The GameObject representing the active state.")]
    public GameObject activePop;
    [Tooltip("The BoxCollider while active.")]
    public BoxCollider activeCollider;
    [Tooltip("The BoxCollider while inactive")]
    public BoxCollider inactiveCollider;
    [Tooltip("Whether or not to default to inactive state while being dragged.")]
    public bool inactiveWhileDragging;

    private bool beingDragged; //Whether or not this is currently being dragged by HandDraggable.
    private TagManager tagManager; //The Tag Manager
    
    /// <summary>
    /// At Awake, set inactive and instantiate variables.
    /// </summary>
    void Awake()
    {
        inactivePop.SetActive(true);
        inactiveCollider.enabled = false;
        activePop.SetActive(false);
        activeCollider.enabled = true;
        beingDragged = false;
        tagManager = TagManager.instance;
    }

    /// <summary>
    /// Add listeners for drag events.
    /// </summary>
    void Start()
    {
        this.gameObject.GetComponent<HandDraggable>().StartedDragging += () =>
        {
            OnDragStart();
        };
        this.gameObject.GetComponent<HandDraggable>().StoppedDragging += () =>
        {
            OnDragStop();
        };
    }
    
    /// <summary>
    /// When entering focus, PopIn after 1 second
    /// </summary>
    public void OnFocusEnter()
    {
        Invoke("PopIn", 1);
    }

    /// <summary>
    /// When exitign focus, PopOut after 1 second
    /// </summary>
    public void OnFocusExit()
    {
        if (tagManager.GetTagInFocus() == this.gameObject)
        {
            tagManager.SetTagInFocus(null);
        }
        Invoke("PopOut", 1);
    }

    /// <summary>
    /// This function handles "Popping In" to the active state.
    /// If not inactiveWhileBeingDragging and being dragged,
    /// Set the TagManager's tagInFocus to this and 
    /// Enable the active pop and collider and disable the active pop and collider.
    /// </summary>
    public void PopIn()
    {
        if (!inactiveWhileDragging || (inactiveWhileDragging && !beingDragged))
        {
            tagManager.SetTagInFocus(this.gameObject);
            activePop.SetActive(true);
            activeCollider.enabled = true;
            //inactivePop.GetComponent<FadeObjectInOut>().FadeOut();
            //activePop.GetComponent<FadeObjectInOut>().FadeIn();
            inactivePop.SetActive(false);
            inactiveCollider.enabled = false;
        }
    }
    
    /// <summary>
    /// This method handles "Popping Out" of focus.
    /// Remove this from the TagManager's tagInFocus,
    /// Disable the active pop and collider and enable the inactive ones.
    /// </summary>
    public void PopOut()
    {
        activePop.SetActive(false);
        activeCollider.enabled = false;
        //inactivePop.GetComponent<FadeObjectInOut>().FadeOut();
        //activePop.GetComponent<FadeObjectInOut>().FadeIn();
        inactivePop.SetActive(true);
        inactiveCollider.enabled = true;
    }

    /// <summary>
    /// If dragging starts on this object and inactiveWhileDragging is true,
    /// PopOut into inactive mode and set beingDragged to true.
    /// </summary>
    public void OnDragStart()
    {
        if (inactiveWhileDragging)
        {
            PopOut();
            beingDragged = true;
        }
    }

    /// <summary>
    /// If inactiveWhileDragging, set beingDragged to false when dragging stops.
    /// Also checks to see if this is the tag in focus and if so pops back in when dragging is done
    /// </summary>
    public void OnDragStop()
    {
        if (inactiveWhileDragging)
        {
            beingDragged = false;
            if(tagManager.GetTagInFocus() == this.gameObject) { PopIn(); }
        }
    }
}