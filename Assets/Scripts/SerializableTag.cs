using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Serializable representation of the Tag objects
/// </summary>
[System.Serializable]
public class SerializableTag 
{
    [Newtonsoft.Json.JsonProperty]
    public string tagName;
    [Newtonsoft.Json.JsonProperty]
    public EnumTagTypes tagType;
    [Newtonsoft.Json.JsonProperty]
    public string tagText;

    public SerializableTag()
    {
        this.tagName = "NoName";
        this.tagType = EnumTagTypes.Fault;
        this.tagText = "NoText";
    }


    public SerializableTag(string name, EnumTagTypes type, string text)
    {
        this.tagName = name;
        this.tagType = type;
        this.tagText = text;
    }

    public string GetName() { return tagName; }
    public EnumTagTypes GetType() { return tagType; }
    public string GetText() { return tagText; }

    public void SetName(string name) { tagName = name; }
    public void SetType(EnumTagTypes type) { tagType = type; }
    public void SetText(string text) { tagText = text; }




}
