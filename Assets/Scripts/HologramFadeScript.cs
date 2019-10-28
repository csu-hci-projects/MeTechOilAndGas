using System.Collections;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class HologramFadeScript : MonoBehaviour, IFocusable
{
    [Tooltip("Color of object while in focus")]
    public Color focusedColor;
    [Space(10)]
    [Tooltip("How long fading in takes")]
    public float fadeInTime = 1;
    [Tooltip("How long fading out takes")]
    public float fadeOutTime = 1;

    private float progress; //How far in fading object is

    private Material material; //Material of object
    private Color unfocusedColor; //Color while out of focus

    private Coroutine coroutine; //Coroutine to handle fading

    /// <summary>
    /// Get's gameObject's material and color to set as unfocused color.
    /// </summary>
    void Start()
    {
        material = GetComponent<Renderer>().material;
        unfocusedColor = material.GetColor("_Color");
    }

    /// <summary>
    /// When entering focus, stop the current corouting and start a Fade in Coroutine
    /// </summary>
    public void OnFocusEnter()
    {
        StopCurrentCoroutineIfActive();
        coroutine = StartCoroutine(CountProgress(fadeInTime));
    }

    /// <summary>
    /// When exiting focus, stop the current corouting and start a Fade out Coroutine
    /// </summary>
    public void OnFocusExit()
    {
        StopCurrentCoroutineIfActive();
        coroutine = StartCoroutine(CountProgress(fadeOutTime * -1));
    }

    /// <summary>
    /// If in progress, stop the current coroutine
    /// </summary>
    private void StopCurrentCoroutineIfActive()
    {
        if (progress != 0 && progress != 1)
        {
            StopCoroutine(coroutine);
        }
    }

    /// <summary>
    /// Determine how much progress has been made, based on the increment of time. Set the color based on that.
    /// </summary>
    /// <param name="time">How much time has passed</param>
    /// <returns>Null on yield</returns>
    IEnumerator CountProgress(float time)
    {
        while (progress >= 0 && progress <= 1)
        {
            float increment = Time.deltaTime / time;
            progress += increment;
            material.SetColor("_Color", Color.Lerp(unfocusedColor, focusedColor, progress));
            yield return null;
        }
        progress = Mathf.Clamp(progress, 0, 1);
    }
}