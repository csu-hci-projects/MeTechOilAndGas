using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// This script is a Listener for the DictationHandler. 
/// It handles the inputs to start and stop dictation
/// And handles getting the recorded and predicted text to the right places.
/// </summary>
public class DictationListener : MonoBehaviour, IInputClickHandler
{

    

    [Tooltip("The prefab this object will use to display messages about dictation.")]
    public GameObject dictationHUDPrefab;
    [Tooltip("How far below this.gameObject's Y axis to spawn the dictaionHUDPrefab.")]
    public float HUDOffsetY;
    [Tooltip("The TextMeshPro to add recorded text to.")]
    public TextMeshPro tagText;

    private DictationHandler dictationHandler; //The dictation handler this listens to
    private GameObject dictationHUD; //The instance of the dictationHUDPrefab
    private TextMeshPro dictationHUDText; //The dictationHUD's TextMeshPro
    private bool activeHUD; //Whether or not the HUD is active
    private string recordingMessage; //Message to notify of recording
    private string dismissMessage; //Message to notify of dismissal
    private string priorText; //Text that was previously in the tagText, to be appended to instead of replaced.
    private HandDraggable handDraggable; //HandDraggable scipt

    /// <summary>
    /// Instantiate private vars
    /// </summary>
    private void Awake()
    {
        activeHUD = false;
        dismissMessage = "\n Say 'Stop Recording' or tap to close.";
        recordingMessage = "Recording... Tap on tag to stop. \n";
        priorText = "";
        handDraggable = this.gameObject.GetComponent<HandDraggable>();
    }

    /// <summary>
    /// Instantiate the DictationHandler after it has awoken
    /// </summary>
    public void Start()
    {

        dictationHandler = DictationHandler.instance;
    }

    /// <summary>
    /// The function to start recording. If not already recording,
    /// Shuts down PhraseRecognition, starts the DictationHandler and subscribes to it.
    /// Text already in the tag is saved, the HUD is spawned, and HandDraggable is disabled
    /// On the parent gameObject to allow tapping to stop dictation.
    /// </summary>
    public void StartRecording()
    {
        if (!dictationHandler.IsRecording())
        {
            PhraseRecognitionSystem.Shutdown();
            dictationHandler.StartRecording();
            dictationHandler.Subscribe(this);
            priorText = tagText.text;
            SpawnHUD();
            //Turn off Hand Draggable
            if (handDraggable != null)
            {
                handDraggable.enabled = false;
            }
        }
    }


    /// <summary>
    /// If recording, this function stops it. The DictationHandler's StopRecording is called,
    /// This unsubscribes, the PhraseRecognition System is restarted, this.gameObject's
    /// HandDraggable is reinabled and the HUD is deleted.
    /// </summary>
    public void StopRecording()
    {
        if (activeHUD)
        { 
        dictationHandler.StopRecording();
        dictationHandler.Unsubscribe(this);
        PhraseRecognitionSystem.Restart();
        priorText = "";
        }
        DeleteHUD();
        if (handDraggable != null)
        {
            handDraggable.enabled = true;
        }

    }

    /// <summary>
    /// Function for the DictationHandler to notify this of an updated prediction.
    /// If the HUD is active, displays the prediction on there.
    /// </summary>
    /// <param name="prediction">The current dictation prediction</param>
    public void UpdatePrediction(string prediction)
    {
        if (activeHUD)
        {
            dictationHUDText.text = recordingMessage + "Prediction: " + prediction;
        }
    }

    /// <summary>
    /// Function for the DictationHandler to notify this of an update to the recorded text.
    /// Updates the tagText with the new recorded text, adding it to what was there before recording.
    /// </summary>
    /// <param name="prediction">The finalized recorded text</param>
    public void UpdateRecorded(string recorded)
    {
        if (activeHUD)
        {
            //Somehow put recorded text on tag
            tagText.text = priorText + "\n" + recorded;
            dictationHUDText.text = recordingMessage;
        }
    }

    /// <summary>
    /// Function for the DictationHandler to notify user of errors in dictation.
    /// Displays thhe given error message in the dictationHUD.
    /// </summary>
    /// <param name="error">The error message from the DictationHandler</param>
    public void NotifyError(string error)
    {
        if (activeHUD)
        {
            dictationHUDText.text = "Error! " + error + dismissMessage;
        }
    }

    /// <summary>
    /// Function for the DictationHandler to notify user of dictation ending
    /// For a reason other than user input. Displays Cause on dictationHUD.
    /// </summary>
    /// <param name="cause">The reason for DictationCompletion</param>
    public void NotifyDone(DictationCompletionCause cause)
    {
        if (activeHUD)
        {
            dictationHUDText.text = "Recording stopped: " +cause + dismissMessage;
        }
    }


    /// <summary>
    /// If the HUD is inactive, sets it to active and spawns a new HUD
    /// Directly below this.gameObject's location, determined by HUDOffsetY.
    /// Set's the dictationHUD's text to the recording message.
    /// </summary>
    private void SpawnHUD()
    {
        if (!activeHUD)
        {
            activeHUD = true;
            Vector3 spawnPosition =  new Vector3(gameObject.transform.localPosition.x,
                gameObject.transform.localPosition.y - HUDOffsetY,
                gameObject.transform.localPosition.z);
            dictationHUD = Instantiate(dictationHUDPrefab);
            dictationHUD.transform.position = spawnPosition;
            dictationHUD.transform.localRotation = gameObject.transform.localRotation;
            RectTransform rect = dictationHUD.GetComponent<RectTransform>();
            rect.localPosition = spawnPosition;
            rect.transform.localRotation = gameObject.transform.localRotation;
            dictationHUDText = dictationHUD.GetComponent<TextMeshPro>();
            dictationHUDText.text = recordingMessage;
        }
    }
    

    /// <summary>
    /// If the HUD is active, this sets it inactive and destroys it.
    /// </summary>
    private void DeleteHUD()
    {
        if (activeHUD)
        {
            activeHUD = false;
            Destroy(dictationHUD);
            dictationHUD = null;
            dictationHUDText = null;
        }
    }

    /// <summary>
    /// When this.GameObject is tapped while the HUD is active,
    /// Either because the DictationHandler is recording or has stopped for another reason
    /// This function stops recording.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (activeHUD)
        {
            StopRecording();
        }
    }


}
