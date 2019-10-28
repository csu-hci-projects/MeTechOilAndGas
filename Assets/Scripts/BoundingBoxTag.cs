using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//Denotes class as serializable
///<summary>
///This class exists as an serializable representation of a bounding box. 
/// </summary>
public class BoundingBoxTag
{
    [Newtonsoft.Json.JsonProperty] //Marks variable as a JSON property

    ///<summary>
    ///The name of the bounding box object. Used at reload to retireve world anchors
    /// </summary>
    public string boxName; 
    [Newtonsoft.Json.JsonProperty]
    ///<summary>
    ///The scale of the bounding box, based on GameObject.transform.localScale. Applied to the bounding box object at reload
    /// </summary>
    public SerializableVector3 boxScale; 

    
    public BoundingBoxTag()
    {
        this.boxName = "NoName";
        this.boxScale = new Vector3();
    }

    public BoundingBoxTag(string name, Vector3 scale)
    {
        this.boxName = name;
        this.boxScale = scale;
    }

    public string GetName()
    {
        return boxName;
    }

    public Vector3 GetScale()
    {
        return boxScale;
    }

    public void SetName(string name)
    {
        this.boxName = name;
    }

    public void SetScale(Vector3 scale)
    {
        this.boxScale = scale;
    }




}
