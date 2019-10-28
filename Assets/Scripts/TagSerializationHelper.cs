using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// This class works as a middle man between SerializableTag and the multiple parts of the tag prefabs
/// To make instantiation of Serializable tag easier.
/// </summary>
public class TagSerializationHelper : MonoBehaviour
{
    [Tooltip("The type of tag this script is attached to.")]
    public EnumTagTypes tagType;
    [Tooltip("The TextMeshPro holding the text for this tag.")]
    public GameObject tagLabel;
    private TextMeshPro textMeshPro; //Reference to the TextMeshPro

    /// <summary>
    /// Set textMeshPro;
    /// </summary>
    private void Awake()
    {
        textMeshPro = tagLabel.GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// Getter Method for the tag text
    /// </summary>
    /// <returns>The text currently in the textMeshPro</returns>
    public string GetText()
    {
        return textMeshPro.text;
    }

    /// <summary>
    /// Setter Method for the Tag's text. Sets it in the textMeshPro.
    /// </summary>
    /// <param name="text">The text to set the tagText to.</param>
    public void SetText(string text)
    {
        textMeshPro.text = text;
    }

    /// <summary>
    /// Getter method for the Tag Type.
    /// </summary>
    /// <returns>EnumTagType representing the tag's type.</returns>
    public EnumTagTypes GetType()
    {
        return tagType;
    }

    /// <summary>
    /// Getter method for the tag name.
    /// </summary>
    /// <returns>The name of the gameObject.</returns>
    public string GetName()
    {
        return this.gameObject.name;
    }
    



}
