using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsScriptibleObject : MonoBehaviour
{
    [SerializeField]
    public int maxSlots;
   
    public float maxWeight;
    public float Weight;

    public List<InventoryController> Inventory = new List<InventoryController>();



    // the quantity coming from the object in the scene and not the scriptible object
    // and the scriptible object coming fromt he item type as an enum also the material and such
    public void AddItem(InventoryController item,int quantity)
    {

        
    while(item.quantity > 0)
        {
            if (maxWeight > Weight + item.weight && maxSlots > Inventory.Count)
            {
                if (item.IsStackable)
                {
                    InventoryController exisingItem = Inventory.Find(joiningItem => joiningItem.itemName == item.itemName);
                    if (exisingItem != null)
                    {
                        exisingItem.quantity += 1;
                    }
                    else
                    {

                        Inventory.Add(item);
                    }
                    Weight += item.weight;
                    item.quantity--;

                }
                else
                {

                    Inventory.Add(item);
                    Weight += item.weight;
                    break;
                }
                
               
            }
            else
            {
                Debug.Log("Cannot pickup this item");
                break;
                
            }
        }
        


        
    }


}

