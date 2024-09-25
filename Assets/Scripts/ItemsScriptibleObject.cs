using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Check the create asset menu
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class InventoryController : ScriptableObject
{
    public string itemName;
    public int weight;
    public string description;
    public Sprite Icon;
    public bool IsStackable;
    public int quantity;
    public enum ItemType
    { Armor, Weapon, Consumable, Material };
    public int durability;

}

