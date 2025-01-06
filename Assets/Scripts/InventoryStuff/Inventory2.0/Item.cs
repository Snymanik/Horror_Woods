using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public string description;
    public Texture Icon;
    public GameObject gobject;
    

    public abstract Item GetItem();
    public abstract Item GetTool();
    public abstract Item GetMisc();
    public abstract Item GetFood();
}
