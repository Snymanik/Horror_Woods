using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tool", menuName = "Items/Tool")]
public class Tool : Item
{
    [Header("Tool")]
    //Should I do private and do getter and setter
    [SerializeField] private float ToolEfficiency; 
    public float ToolEff => ToolEfficiency;
    public ToolType Tooltype;

    public enum ToolType
    {
        Weapon,
        Pickaxe,
        Axe,
        Torch

    }
     

    public override Item GetItem() { return this;  }
    public override Item GetTool() { return this; }
    public override Item GetMisc() { return null; }
    public override Item GetFood() { return null; }
}
