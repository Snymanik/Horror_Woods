using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Misc", menuName = "Items/Misc")]
public class Misc : Item
{
    //[Header("Misc")]
    //public int quantity;

    public override Item GetItem() { return this; }
    public override Item GetTool() { return null; }
    public override Item GetMisc() { return this; }
    public override Item GetFood() { return null; }
}
