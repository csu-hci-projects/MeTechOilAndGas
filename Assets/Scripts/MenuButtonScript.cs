using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtonScript : MonoBehaviour, IInputClickHandler
{
    [Tooltip("The action to perform when this button is tapped.")]
    public UnityEvent onInputAction;

    /// <summary>
    /// When this button is tapped, perform the givin input.
    /// </summary>
    /// <param name="eventData">Event data of the tap</param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("tap recognised");
        onInputAction.Invoke();
    }




}
