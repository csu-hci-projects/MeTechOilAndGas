using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;

/// <summary>
/// This class handles the saving and loading of objects
/// Specifically, the lists of Tags and Bounding Boxes. 
/// SerializableTag and BoundingBoxTag are used to represent these specifically
/// </summary>
public static class SaveLoad
{
    /// <summary>
    /// Generic load function called by specialized load functions.
    /// Using the Application.persistentDataPath, searches for the specified file
    /// And if it exists, reads the Bytes into a byte[] and converts it to a json.
    /// </summary>
    /// <param name="filename">The filename of the file to be loaded</param>
    /// <returns>
    /// json, the Json of loaded data, to be deserialiazed by called function.
    /// Null if file does not exist of exception occurs.
    /// </returns>
    private static string Load(string filename)
    {
        try
        {
            string path = Application.persistentDataPath + "\\" + filename;
            if (File.Exists(path)) { 
                byte[] data = UnityEngine.Windows.File.ReadAllBytes(path);
                string json = Encoding.ASCII.GetString(data);
                Debug.Log("Loading... : " + json);
                return json;
            }
            else
            {
                Debug.Log("Could not find file to load! " + filename);
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error while Loading!" + ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// Generic save function. Saves the given Json at the specified filename
    /// As a byte[] written to a file at the persistentDataPath.
    /// </summary>
    /// <param name="json">The serialized json to be saved</param>
    /// <param name="filename">The name of the file to save it in</param>
    private static void Save(string json, string filename)
    {
        try
        {
            string path = Application.persistentDataPath + "\\" + filename;
            Debug.Log("Saving... : " + json);
            byte[] data = Encoding.ASCII.GetBytes(json);
            UnityEngine.Windows.File.WriteAllBytes(path, data);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error while saving!" + ex.ToString());
        }
        
    }

    /// <summary>
    /// Specialized save function for BoundingBoxTags.
    /// Serializes the list of BoundingBoxTags to a Json
    /// And passes it to the generic save function.
    /// </summary>
    /// <param name="bBtags">List of serializable BoundingBoxTags to be saved.</param>
    public static void SaveBBTags(List<BoundingBoxTag> bBtags)
    {
        foreach(BoundingBoxTag tag in bBtags) { Debug.Log(tag.GetName()); }
        string json = JsonConvert.SerializeObject(bBtags, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
        Save(json, "BoundingBoxTags.json");
    }

    /// <summary>
    /// Specialized load function for BoundingBoxTags.
    /// Calls the generic load function with the BoundingBoxTags file 
    /// And deserializes the returned Json into a list of BoundingBoxTags that is then returned.
    /// </summary>
    /// <returns>
    /// savedBBTags, The list of saved BoundingBoxTags
    /// Null if file not found.
    /// </returns>
    public static List<BoundingBoxTag> LoadBBTags()
    {
        string json = Load("BoundingBoxTags.json");
        if (json == null) { return new List<BoundingBoxTag>(); }
        List<BoundingBoxTag> savedBBTags = JsonConvert.DeserializeObject<List<BoundingBoxTag>>(json);
        return savedBBTags;
    }

    /// <summary>
    /// Specialized load function for SerializableTags.
    /// Calls the generic load function with the SerializableTags file 
    /// And deserializes the returned Json into a list of SerializableTags that is then returned.
    /// </summary>
    /// <returns>
    /// savedTags, The list of saved Serializable tags
    /// Null if file not found.
    /// </returns>
    public static List<SerializableTag> LoadTags()
    {
        string json = Load("SerializableTags.json");
        if (json == null) { return new List<SerializableTag>(); }
        List<SerializableTag> savedTags = JsonConvert.DeserializeObject<List<SerializableTag>>(json);
        return savedTags;
    }

    /// <summary>
    /// Specialized save function for SerializableTags.
    /// Serializes the list of SerializableTags to a Json
    /// And passes it to the generic save function.
    /// </summary>
    /// <param name="tags">List of SerializableTags to be saved.</param>
    public static void SaveTags(List<SerializableTag> tags)
    {
        foreach (SerializableTag tag in tags) { Debug.Log(tag.tagName); }
        string json = JsonConvert.SerializeObject(tags, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
        Save(json, "SerializableTags.json");
    }

}
