using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.XR.WSA.Input;


/// <summary>
/// This class works as a middleman between the DictationListener and the DictationRecognizer,
/// Doing the heavy lifting. Implemented due to issues using IDictationListener.
/// </summary>
public class DictationHandler : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer;
    public string predictedText; //The prediction from the dictationRecognizer
    public string recordedText; //Finalized text that has been transcribed
    private bool isRecording;
    public static DictationHandler instance; //Allows class to behave as a singleton
    private List<DictationListener> DictationListeners; //Implements Observer Pattern with DictationListeners

    void Awake()
    {
        instance = this;
        predictedText = "";
        recordedText = "";
        DictationListeners = new List<DictationListener>();
    }


    /// <summary>
    /// If not already recording, set text to empty, initialize the recognizer, and start recording.
    /// </summary>
    public void StartRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            predictedText = "";
            recordedText = "";
            InitDictationRecognizer();
            dictationRecognizer.Start();
        }
    }

    /// <summary>
    /// If recording, stop the dictation recognizer and destroy it so that Voice commands can resume.
    /// </summary>
    public void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
        }
    }

    /// <summary>
    /// Handles initialization of new dictation recognizers.
    /// Necessary since dictation recognizers must be destroyed to resume voice control
    /// Creates a new dictation recognizer and sets this class' functions as listeners.
    /// </summary>
    public void InitDictationRecognizer()
    {
        dictationRecognizer = new UnityEngine.Windows.Speech.DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
    }

    /// <summary>
    /// A listener for when the DictationRecognizer had finalized its prediction
    /// And has transcribed the recorded text as a result. Adds that to recordedText.
    /// Updates the dictation listeners with all the recorded text so far.
    /// </summary>
    /// <param name="text">The final recorded text returned by the DictationRecognizer</param>
    /// <param name="confidence">How confident the DR is in the text being accurate</param>
    public void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        recordedText += text + " ";
        foreach(DictationListener dictationListener in DictationListeners)
        {
            dictationListener.UpdateRecorded(recordedText);
        }
    }

    /// <summary>
    /// A listener for when the DictationRecognizer has made a prediction
    /// Updates the dictation listeners with the prediction
    /// </summary>
    /// <param name="text">The text for the current prediction</param>
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        predictedText = text;
        foreach (DictationListener dictationListener in DictationListeners)
        {
            dictationListener.UpdatePrediction(predictedText);
        }
    }

    /// <summary>
    /// A listener for when the DictationRecognizer had finished recording.
    /// If not because the user turned it off, it will call this class'
    /// StopRecording function and notify the Dictation Listeners of the reason.
    /// </summary>
    /// <param name="cause">The reason that dictation has completed</param>
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        
        if(cause != DictationCompletionCause.Complete){
            StopRecording();
            foreach (DictationListener dictationListener in DictationListeners)
            {
                dictationListener.NotifyDone(cause);
            }
        }
    }

    /// <summary>
    /// A listener for when the DictationRecognizer has an error.
    /// Stops dictation and notifies the DictationListeners of said error.
    /// </summary>
    /// <param name="error">The error code of the dictation</param>
    /// <param name="hresult">The result code for the error</param>
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        StopRecording();
        foreach (DictationListener dictationListener in DictationListeners)
        {
            dictationListener.NotifyError(error);
        }
        
    }

    /// <summary>
    /// Allows DictationListeners to subscribe for updates through the Observer pattern
    /// </summary>
    /// <param name="dictationListener">The dictationListener subscribing</param>
    public void Subscribe(DictationListener dictationListener)
    {
        DictationListeners.Add(dictationListener);
    }

    /// <summary>
    /// Allows DictationListeners to unsubscribe from updates through the Observer pattern
    /// </summary>
    /// <param name="dictationListener">The dictationListener unsubscribing</param>
    public void Unsubscribe(DictationListener dictationListener)
    {
        DictationListeners.Remove(dictationListener);
    }

    /// <summary>
    /// If a TapHandler is implemented, allows taps to start and stop recording as appropriate.
    /// </summary>
    /// <param name="obj">The arguments for the tap event</param>
    private void TapHandler(TappedEventArgs obj)
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    /// <summary>
    /// Public getter method for whether or not the DictationHandler is currently recording.
    /// </summary>
    /// <returns>True if recording, false if not</returns>
    public bool IsRecording()
    {
        return isRecording;
    }
}
