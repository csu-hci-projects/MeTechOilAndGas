using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all things Tags and Bounding Boxes,
/// Including Spawning, Saving, Loading, Deleting,
/// And acting as a connector between SpeechManager and the TagInFocus.
/// </summary>
public class TagManager : MonoBehaviour
{

    public static TagManager instance; //Allows behavior as singleton

    [Tooltip("The prefab for bounding box tags")]
    public GameObject boundingBoxTag;
    [Tooltip("The prefab for FaultTags. Must have script 'TagSerializationHelper'.")]
    public GameObject faultTag;
    [Tooltip("The prefab for History Tags. Must have script 'TagSerializationHelper'.")]
    public GameObject historyTag;
    [Tooltip("The prefab for Label Tags. Must have script 'TagSerializationHelper'.")]
    public GameObject labelTag;
    [Tooltip("The menu, used to spawn tags at the menu's rotation.")]
    public GameObject menu;

    private int faultTagCount; //The number of fault tags. Used for naming.
    private int historyTagCount; //The number of history tags. Used for naming.
    private int labelTagCount; //The number of label tags. Used for naming.

    private List<GameObject> boundingBoxTags; //References to each created bounding box
    private List<GameObject> tagList; //References to each tag that has 'TagSerializationHelper"
    private GameObject tagInFocus; //The tag currently in focus.
    private bool tagInFocusIsBoundingBox; //Whether or not the tag in focus is a BoundingBox.
    
    /// <summary>
    /// Instantiates variables, and reloads any Tags and BoundingBoxes that have been saved.
    /// </summary>
    private void Awake()
    {
        instance = this;
        faultTagCount = 0;
        historyTagCount = 0;
        labelTagCount = 0;

        //Load Bounding Boxes
        boundingBoxTags = new List<GameObject>();
        List<BoundingBoxTag> savedBBTags = SaveLoad.LoadBBTags();
        if (savedBBTags.Count > 0)
        {
            ReloadBBTags(savedBBTags);
        }
        //Load Tags
        tagList = new List<GameObject>();
        List<SerializableTag> savedTags = SaveLoad.LoadTags();
        if (savedTags.Count > 0)
        {
            ReloadTags(savedTags);
        }

    }
    
    /// <summary>
    /// This function is used for spawning new BoundingBoxes.
    /// They are instantiated from the boundingBoxPrefab,
    /// Named based on the number of BoundingBoxes, and placed 2 meters in front of the camera.
    /// The new box is given an anchor script, added to the list, and the list is saved.
    /// </summary>
    public void SpawnBBTag()
    {
        Debug.Log("Spawning Boundary Box...");
        GameObject bBTag = (GameObject)Instantiate(boundingBoxTag);
        string name = "BoundingBoxTag" + boundingBoxTags.Count;
        bBTag.name = name;
        bBTag.transform.localPosition = Camera.main.transform.localPosition+Camera.main.transform.forward*2;
        bBTag.AddComponent<AnchorScript>();
        boundingBoxTags.Add(bBTag);
        SaveBBTags();
    }

    /// <summary>
    /// This function is used for spawning new Tags.
    /// They are instantiated from the appropiate prefab of their type,
    /// Named based on the type and number of that type's tags, 
    /// and placed 2 meters in front of the camera.
    /// The new tag is given an anchor script, added to the list, and the list is saved.
    /// </summary>
    /// <param name="tagType">The type of tag that is to be spawned.</param>
    private void SpawnTag(EnumTagTypes tagType)
    {
        GameObject tag = null;
        switch (tagType)
        {
            case EnumTagTypes.Fault:
                tag = (GameObject)Instantiate(faultTag);
                tag.name = "FaultTag" + faultTagCount;
                faultTagCount++;
                break;
            case EnumTagTypes.History:
                tag = (GameObject)Instantiate(historyTag);
                tag.name = "HistoryTag" + historyTagCount;
                historyTagCount++;
                break;
            case EnumTagTypes.Label:
                tag = (GameObject)Instantiate(labelTag);
                tag.name = "LabelTag" + labelTagCount;
                labelTagCount++;
                break;
        }
        tag.transform.localPosition = Camera.main.transform.localPosition + Camera.main.transform.forward * 2;
        tag.transform.localRotation = menu.transform.localRotation;
        tag.AddComponent<AnchorScript>();
        tagList.Add(tag);
        SaveTags();
    }

    /// <summary>
    /// Function to spawn Fault Tags, for use by menu and voice commands
    /// </summary>
    public void SpawnFaultTag()
    {
        Debug.Log("Spawning Fault...");
        SpawnTag(EnumTagTypes.Fault);
    }

    /// <summary>
    /// Function to spawt History Tags, for use by menu and voice commands
    /// </summary>
    public void SpawnHistoryTag()
    {
        Debug.Log("Spawning History...");
        SpawnTag(EnumTagTypes.History);
    }

    /// <summary>
    /// Function to spawn Label Tags, for use by menu and voice commands
    /// </summary>
    public void SpawnLabelTag()
    {
        Debug.Log("Spawning Label...");
        SpawnTag(EnumTagTypes.Label);
    }

    ///<summary>
    ///Function to save all BoundingBoxes in the boundingBoxTags list.
    ///A list of serializable BoundingBoxTags is made,
    ///Each given the name and transform.localScale of their associated BoundingBox.
    ///These are passed to SaveLoad.SaveBBTags to be saved to the device.
    /// </summary>   
    public void SaveBBTags()
    {
        List<BoundingBoxTag> savedBBTags = new List<BoundingBoxTag>();
        foreach (GameObject tag in boundingBoxTags)
        {
            savedBBTags.Add(new BoundingBoxTag(tag.name, tag.transform.localScale));
        }
        SaveLoad.SaveBBTags(savedBBTags);
    }

    ///<summary>
    ///Function to save all SerializableTags in the tagList.
    ///A list of SerializableTags is made,
    ///Each given the name, type, and text of their associated Tag.
    ///These are passed to SaveLoad.SaveTags to be saved to the device.
    /// </summary> 
    public void SaveTags()
    {
        List<SerializableTag> savedTags = new List<SerializableTag>();
        foreach (GameObject tag in tagList)
        {
            TagSerializationHelper helper = tag.GetComponent<TagSerializationHelper>();
            savedTags.Add(new SerializableTag(helper.GetName(), helper.GetType(), helper.GetText()));
        }
        SaveLoad.SaveTags(savedTags);
    }

    /// <summary>
    /// This class takes the loaded list of BBTags, spawns those tags,
    /// Assigns the name to the new BBTag and applies the scale,
    /// And then attaches them to anchors. They are then added to the list.
    /// </summary>
    private void ReloadBBTags(List<BoundingBoxTag> savedBBTags)
    {
        foreach (BoundingBoxTag savedBBTag in savedBBTags)
        {
            GameObject bBTag = (GameObject)Instantiate(boundingBoxTag);
            bBTag.name = savedBBTag.GetName();
            bBTag.AddComponent<AnchorScript>();
            bBTag.transform.localScale = savedBBTag.GetScale();
            boundingBoxTags.Add(bBTag);
        }
    }

    /// <summary>
    /// This class takes the loaded list of Tags, spawns those tags based on type,
    /// Assigns their names and text, and then attaches them to anchors. 
    /// They are then added to the list. The counts of each type are adjusted as tags are spawned.
    /// </summary>
    public void ReloadTags(List<SerializableTag> savedTags)
    {
        foreach(SerializableTag tag in savedTags)
        {
            GameObject newTag = null;
            switch (tag.GetType())
            {
                case EnumTagTypes.Fault:
                    newTag = (GameObject)Instantiate(faultTag);
                    faultTagCount++;
                    break;
                case EnumTagTypes.History:
                    newTag = (GameObject)Instantiate(historyTag);
                    historyTagCount++;
                    break;
                case EnumTagTypes.Label:
                    newTag = (GameObject)Instantiate(labelTag);
                    labelTagCount++;
                    break;
            }
            newTag.name = tag.GetName();
            newTag.GetComponent<TagSerializationHelper>().SetText(tag.GetText());
            newTag.AddComponent<AnchorScript>();
            tagList.Add(newTag);
        }
    }

    /// <summary>
    /// This function deletes the current tagInFocus if it is not a BoundingBox.
    /// The tagInFocus is deleted from the list, the count for their type is decremented, 
    /// And the tagInFocus is destroyed. The current list is then saved.
    /// </summary>
    public void DeleteTag() {
        if (tagInFocus != null && !tagInFocusIsBoundingBox)
        {
            tagInFocus.SetActive(false);
            tagList.Remove(tagInFocus);
            switch (tagInFocus.GetComponent<TagSerializationHelper>().GetType())
            {
                case EnumTagTypes.Fault:
                    faultTagCount--;
                    break;
                case EnumTagTypes.History:
                    historyTagCount--;
                    break;
                case EnumTagTypes.Label:
                    labelTagCount--;
                    break;
            }
            DestroyTag();
            SaveTags();
        }
    }

    /// <summary>
    /// This function deletes the current tagInFocus if it is a BoundingBox.
    /// The tagInFocus is deleted from the list, 
    /// And the tagInFocus is destroyed. The current list is then saved.
    /// </summary>
    public void DeleteBBTag()
    {
        if (tagInFocus!=null && tagInFocusIsBoundingBox){
            boundingBoxTags.Remove(tagInFocus);
            DestroyTag();
            SaveBBTags();
        }
    }

    /// <summary>
    /// This is an emergency function to delete every single tag.
    /// Should only be used for debugging or in extreme cases.
    /// </summary>
    public void DeleteAllTags()
    {
        while(boundingBoxTags.Count > 0) {
            GameObject bbTag = boundingBoxTags[0];
            boundingBoxTags.Remove(bbTag);
            WorldAnchorManager.Instance.DeleteAnchor(bbTag.name);
            Destroy(bbTag);
        }
        while(tagList.Count > 0)
        {
            GameObject tag = tagList[0];
            tagList.Remove(tag);
            WorldAnchorManager.Instance.DeleteAnchor(tag.name);
            Destroy(tag);
        }
        faultTagCount = 0;
        historyTagCount = 0;
        labelTagCount = 0;
        SetTagInFocus(null);
        SaveBBTags();
        SaveTags();
    }


    /// <summary>
    /// Function to destroy the tagInFocus.
    /// Deletes the WorldAnchor and sets the tagInFocus to null as well.
    /// </summary>
    public void DestroyTag()
    {
        WorldAnchorManager.Instance.DeleteAnchor(tagInFocus.name);
        Destroy(tagInFocus);
        SetTagInFocus(null);
    }

    /// <summary>
    /// Iterates through the list of BoundingBoxes and returns a list with the names of each.
    /// </summary>
    /// <returns>names, a string list with the names of all Bounding Boxes</returns>
    public List<string> GetBBTagNames()
    {
        if (boundingBoxTags != null)
        {
            List<string> names = new List<string>();
            foreach (GameObject bBTag in boundingBoxTags)
            {
                names.Add(bBTag.name);
            }
            return names;
        }
        else { return new List<string>(); }
    }

    /// <summary>
    /// Iterates through the list of Tags and returns a list with the names of each.
    /// </summary>
    /// <returns>names, a string list with the names of all tags.</returns>
    public List<string> GetTagNames()
    {
        if (tagList != null)
        {
            List<string> names = new List<string>();
            foreach (GameObject tag in tagList)
            {
                names.Add(tag.name);
            }
            return names;
        }
        else { return new List<string>(); }
    }

    /// <summary>
    /// Getter method for the current tagInFocus
    /// </summary>
    /// <returns></returns>
    public GameObject GetTagInFocus()
    {
        return tagInFocus;
    }
    
    /// <summary>
    /// Setter method for the tag in focus.
    /// If the passed Object does not have TagSerializationHelper 
    /// It is considered a Bounding Box and that bool is set True.
    /// Otherwise it is set to false.
    /// </summary>
    /// <param name="tag">The tag that is now in focus.</param>
    public void SetTagInFocus(GameObject tag)
    {
        tagInFocus = tag;
        if (tag != null && tag.GetComponent<TagSerializationHelper>() == null)
        {
            tagInFocusIsBoundingBox = true;
        }else { tagInFocusIsBoundingBox = false; }
    }

    /// <summary>
    /// If the tagInFocus is not null and has a textMeshProManager, turns the tagInFocus's page backwards.
    /// </summary>
    public void FocusedTagBackPage()
    {
        if (tagInFocus != null)
        {
            TextMeshProManager textMeshProManager = tagInFocus.GetComponentInChildren<TextMeshProManager>();
            if (textMeshProManager != null)
            {
                textMeshProManager.turnPageBackward();
            }
        }
    }

    /// <summary>
    /// If the tagInFocus is not null and has a textMeshProManager, turns the tagInFocus's page forwards.
    /// </summary>
    public void FocusedTagNextPage()
    {
        if (tagInFocus != null)
        {
            TextMeshProManager textMeshProManager = tagInFocus.GetComponentInChildren<TextMeshProManager>();
            if (textMeshProManager != null)
            {
                textMeshProManager.turnPageForward();
            }
        }
    }

    /// <summary>
    /// If the tagInFocus is not null and has a textMeshProManager, clears the tagInFocus' text.
    /// </summary>
    public void FocusedTagClear()
    {
        if (tagInFocus != null)
        {
            TextMeshProManager textMeshProManager = tagInFocus.GetComponentInChildren<TextMeshProManager>();
            if (textMeshProManager != null)
            {
                textMeshProManager.clearTag();
            }
        }
    }

    /// <summary>
    /// If the tagInFocus is not null and has a DictationListener, hides the menu and starts Dictation.
    /// </summary>
    public void FocusedTagStartDictation()
    {
        if (tagInFocus != null)
        {
            ToggleMenuScript.instance.HideMenu();
            DictationListener dictationListener = tagInFocus.GetComponent<DictationListener>();
            if (dictationListener != null) { 
                dictationListener.StartRecording();
            }
        }
    }

    /// <summary>
    /// Deletes the tagInFocus if not null.
    /// </summary>
    public void DeleteFocusedTag()
    {
        if (tagInFocus != null)
        {
            if (tagInFocusIsBoundingBox) { DeleteBBTag(); }
            else { DeleteTag(); }
        }
    }

    /// <summary>
    /// When the application leaves focus (is paused or closed), saves the lists of BoundingBoxes and Tags.
    /// </summary>
    /// <param name="focus">Whether or not the Application is in focus.</param>
    public void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveBBTags();
            SaveTags();
        }
    }
}
