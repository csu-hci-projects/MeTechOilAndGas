using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// This script handles the connection of WorldAnchors for GameObjects,
/// Allowing them to retain their real-world position when reloaded
/// </summary>
public class AnchorScript : MonoBehaviour
{
    ///<summary>
    ///WorldAnchorManager.Instance.AttachAnchor returns a string that can be used for Debugging.
    ///This function will first search to see if there is a stored anchor that is associated
    ///With this.gameObject.name. If so, it attaches that anchor to the game object.
    ///If not, it creates a new anchor associated with this.gameObject.name and attaches it.
    ///Listeners for the HandDraggable script are also added, to detach and reattach anchors
    ///At the new location when dragging starts and ends respectively.
    /// </summary>
    void Awake()
    {
        
        string temp1 = WorldAnchorManager.Instance.AttachAnchor(this.gameObject);
        Debug.Log("Added initial anchor: " + temp1);

        HandDraggable handDraggable = this.gameObject.GetComponent<HandDraggable>();

        handDraggable.StartedDragging += () =>
        {
            // Removing anchor temporarily when dragging starts
            WorldAnchorManager.Instance.RemoveAnchor(this.gameObject);
        };
        handDraggable.StoppedDragging += () =>
        {
            // Adding back with the same name when dragging ends
            string temp2 = WorldAnchorManager.Instance.AttachAnchor(this.gameObject);
            Debug.Log("Dragging ended. Added anchor " + temp2);
        };
    }
}