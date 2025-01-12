using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventroyMan : MonoBehaviour
{


    [SerializeField] private GameObject slotHolder ;
    [SerializeField] private Item itemToAdd ;
    [SerializeField] private Item itemToRemove ;
    //later delete the serialize field
    [SerializeField] private GameObject[] slots;



    //public List<SlotTrait> Inventory = new List<SlotTrait>(); // Old list of the Inventory changed to SlotTrait Array
    public SlotTrait[] Inventory;
        
    
    private void Start()
    {

       


        slots = new GameObject[slotHolder.transform.childCount];
        Inventory = new SlotTrait[slots.Length];
        for(int i = 0;i< Inventory.Length; i++)
        {
            
                Inventory[i] = new SlotTrait();
            
            
        }

        for(int i = 0; i < slotHolder.transform.childCount; i++)
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

        RefreshUI();


        AddToInventory(itemToAdd);
        //RemoveFromInventory(itemToRemove);



    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
             //fuck_you = slots[i].transform.GetChild(1).GetComponent<Text>();
            
            try
                {
                    slots[i].transform.GetChild(0).GetComponent<RawImage>().enabled = true;
                    slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = Inventory[i].GetItem().Icon;
                if (Inventory[i].GetItem().IsStackable)
                {
                    
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Inventory[i].GetQuantity().ToString();
                }
                else
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";



            }
            catch
                {
                    slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = null;
                    slots[i].transform.GetChild(0).GetComponent<RawImage>().enabled = false;
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                  
                    


            }

            
        }

    }

    // change to bool or not
    
    public void AddToInventory(Item Item) {

        SlotTrait slot = Contains(Item);
        if (slot != null && slot.GetItem().IsStackable)
        {
            slot.AddQuantity(Item.GetItem().quantity);
        }
        else
        {
            for(int i =0;i< Inventory.Length; i++)
            {
                if (Inventory[i].GetItem() == null)
                {
                    Inventory[i] = new SlotTrait(Item,Item.GetItem().quantity);
                    break;
                }
            }
 /*           if(slots.Length > Inventory.Count)
            Inventory.Add(new SlotTrait(Item,Item.GetItem().quantity));*/
            
        }
        RefreshUI();
    }
    
    public void RemoveFromInventory(Item Item) {
        // Inventory.Remove(Item);

        SlotTrait temp = Contains(Item);
        if (temp != null)
        {
            if(temp.GetQuantity() > 1)
            {
                temp.AddQuantity(-1);
            }
            else
            {
                int slotRemoveIndex = 0;
                for(int i = 0;i< Inventory.Length;i++)
                {


                    if (Inventory[i].GetItem() == Item)
                    {
                        slotRemoveIndex = i;
                        break;
                    }
                }
                //Inventory[slotRemoveIndex].Clear();
            }
            
        }
        else
        {
            Debug.Log("No item in inventory");
        }



          

        RefreshUI();
    }
    
    public SlotTrait Contains(Item _item)
    {
        //foreach(SlotTrait slot in Inventory)
        //{
        //    if (slot.GetItem() == _item)
        //    {
        //        return slot;
        //    }
        //}
        return null;
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].GetItem() != _item)
            {
                return Inventory[i];
            }
            return null;
        }
    }
   
}
