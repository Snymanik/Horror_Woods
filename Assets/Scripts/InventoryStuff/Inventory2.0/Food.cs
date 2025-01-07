using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Food", menuName = "Items/Food")]
public class Food : Item
{
    [Header("Food")]
    //public int quantity;
    public int healthAdd;
    public override Item GetItem() { return this; }
    public override Item GetTool() { return null; }
    public override Item GetMisc() { return null; }
    public override Item GetFood() { return this; }
}
