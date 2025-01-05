using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsScriptibleObject : MonoBehaviour
{
    #region Limits
    [SerializeField]
    public int maxSlots;
    public float maxWeight;
    public float Weight=0;
    #endregion
    public List<InventoryController> Inventory = new List<InventoryController>();
    
    


    // the quantity coming from the object in the scene and not the scriptible object
    // and the scriptible object coming fromt he item type as an enum also the material and such
    public void AddItem(InventoryController item)
    { 
            if (maxWeight > Weight + item.weight)  
            {
                if (item.IsStackable && maxSlots > Inventory.Count)
                {
                    Inventory.Add(item);
                    Weight += item.weight;
                //idk if this will work
                    item.GetComponent<ItemPrefabScript>().prefabQuantity -= 1;
                    


                }
            else
                {
                    InventoryController exisingItem = Inventory.Find(joiningItem => joiningItem.itemName == item.itemName);
                if (exisingItem != null)
                {
                    exisingItem.quantity += 1;
                    //idk if this will work
                    item.GetComponent<ItemPrefabScript>().prefabQuantity -= 1;
                }
                else
                {
                    if (maxSlots > Inventory.Count)
                    {
                        Inventory.Add(item);
                        Weight += item.weight;
                        //idk if this will work
                        item.GetComponent<ItemPrefabScript>().prefabQuantity -= 1;

                    }
                    else
                    {
                        Debug.Log("Cannot add the item");
                    }

                }   
            }

        }


    }


}

