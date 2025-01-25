using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class InventroyMan : MonoBehaviour
{

    [SerializeField] private GameObject itemCursor;

    [SerializeField] private SlotTrait selectedItem;
    private bool itemSelect;

    [SerializeField] private GameObject slotHolder ;
    [SerializeField] private Item itemToAdd ;
    [SerializeField] private Item itemToRemove ;
    private GameObject[] slots;



    [SerializeField] private SlotTrait[] startingItems;
    private SlotTrait[] Inventory;

    private SlotTrait movingSlot;
    private SlotTrait tempSlot;
    private SlotTrait originalSlot;
    bool isMovingItem;
    private void Start()
    {
            
       


        slots = new GameObject[slotHolder.transform.childCount];
        Inventory = new SlotTrait[slots.Length];
        for(int i = 0;i< Inventory.Length; i++)
        {
            
                Inventory[i] = new SlotTrait(null, 0);
            
            
        }
        for (int i = 0; i < slotHolder.transform.childCount; i++)
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        for (int i = 0;i< startingItems.Length; i++)
        {
           // Debug.Log(startingItems[i].GetItem().maxStackSize);
            AddToInventory(startingItems[i].GetItem(), startingItems[i].GetQuantity());
           
        }

        

        RefreshUI();


        AddToInventory(itemToAdd, itemToAdd.GetItem().quantity);
        //RemoveFromInventory(itemToRemove);

        //Debug.Log(Inventory[0] + " CYKA" +  Inventory[0].GetItem() + "BLYAT" + Inventory[0].GetQuantity());

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            itemSelect = !itemSelect;
        }
        if (itemSelect)
        {
            selectedItem = Inventory[15];
        }
        else
        {
            selectedItem = null;
        }



        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if(isMovingItem )
        {
            itemCursor.GetComponent<RawImage>().texture = movingSlot.GetItem().Icon;
            if (movingSlot.GetItem().IsStackable)
            {
                itemCursor.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingSlot.GetQuantity().ToString();
            }
            
        }
        else
        {
            itemCursor.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
            {
                EndMove();
            }
            else
            {
                ItemMove();
            }
            
            
        }
        else if (Input.GetMouseButtonUp(1)) 
        {
            if (isMovingItem)
            {
                EndMove_Single();
            }
            else
            {
                ItemMove_Half();
            }
        }
    }

    #region Inv Utils
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
    
    public void AddToInventory(Item Item, int quantity) {

        if(Item != null)
        {
            SlotTrait slot = Contains(Item);
            // Debug.Log(slot.GetItem());
            if (slot != null)
            {
                int quantityCanAdd  = slot.GetItem().maxStackSize - slot.GetQuantity();
                


                int maxCanAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);
                int remain = quantity - maxCanAdd;
                
                slot.ChangeQuantity(maxCanAdd);
                if (remain > 0)
                {
                    AddToInventory(Item, remain);
                }  

            }
            else
            {
                for (int i = 0; i < Inventory.Length; i++)
                {
                    if (Inventory[i].GetItem() == null)
                    {

                        


                        int quantityCanAdd = Item.GetItem().maxStackSize - quantity;
                        int maxCanAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);
                        int remain = quantity - maxCanAdd;

                        Inventory[i] = new SlotTrait(Item, maxCanAdd);
                        if (remain > 0)
                        {
                            AddToInventory(Item, remain);

                        }






                        break;
                    }
                }
                /*           if(slots.Length > Inventory.Count)
                           Inventory.Add(new SlotTrait(Item,Item.GetItem().quantity));*/

            }
            RefreshUI();
        }
       
        
    }
    
    public void RemoveFromInventory(Item Item) {
        // Inventory.Remove(Item);

        SlotTrait temp = Contains(Item);
        if (temp != null)
        {
            if(temp.GetQuantity() > 1)
            {
                temp.ChangeQuantity(-1);
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
                Inventory[slotRemoveIndex].Clear();
            }
            RefreshUI();
            
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
        //return null;
        
        for (int i = 0; i < Inventory.Length; i++)
        {
            
            if (Inventory[i].GetItem() == _item && Inventory[i].GetItem().IsStackable && Inventory[i].GetQuantity()+_item.GetItem().quantity+2 <= Inventory[i].GetItem().maxStackSize)
            {
                
                return Inventory[i];
            }
            
        }
        return null;
    }
    #endregion Inv Utils

    #region Movement
    private bool ItemMove()
    {
        

        
        originalSlot = GetClosestSlot();
        if(originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotTrait(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }
    private bool ItemMove_Half()
    {

        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }
        
            movingSlot = new SlotTrait(originalSlot.GetItem(), (int)MathF.Ceiling(originalSlot.GetQuantity() / 2f));
            originalSlot.ChangeQuantity(-(int)MathF.Ceiling(originalSlot.GetQuantity() / 2f));
        if (originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }
        //else
        //{
        //    movingSlot = new SlotTrait(originalSlot);
        //    originalSlot.Clear();
        //}

        isMovingItem = true;
        RefreshUI();
        return true;
    }
    private bool EndMove()
    {
        originalSlot = GetClosestSlot();

        if(originalSlot == null)
        {
            AddToInventory(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem() && originalSlot.GetItem().IsStackable && originalSlot.GetQuantity()  < originalSlot.GetItem().maxStackSize)
                {

                    int quantityCanAdd = originalSlot.GetItem().maxStackSize - originalSlot.GetQuantity();
                    int quantityAdding = Mathf.Clamp(movingSlot.GetQuantity(),0,quantityCanAdd);
                    //int remainder = movingSlot.GetQuantity() - quantityAdding;
                    
                    originalSlot.ChangeQuantity(quantityAdding);
                    if (movingSlot.GetQuantity() - quantityAdding <= 0) 
                    {
                        movingSlot.Clear();
                    }
                    else
                    {
                        movingSlot.ChangeQuantity(-quantityAdding);
                        RefreshUI();
                        return false;
                    }




                    //originalSlot.ChangeQuantity(movingSlot.GetQuantity());
                    //movingSlot.Clear();
                    //isMovingItem = false;
                   // RefreshUI();
                }
                else
                {
                    tempSlot = new SlotTrait(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    RefreshUI();

                    return true;

                }

            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
                // isMovingItem = false;

            }
        }
        
        RefreshUI();
        isMovingItem = false;
        return true;
        
    }
    private bool EndMove_Single()
    {
        originalSlot = GetClosestSlot();

     

        if (originalSlot == null )//|| originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem()
        {
            return false;
        }
        
        
        if (originalSlot.GetItem() != null && originalSlot.GetQuantity() >= originalSlot.GetItem().maxStackSize)
        {
            return false;
        } 
        

        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem()) 
        {
            originalSlot.ChangeQuantity(1);
            movingSlot.ChangeQuantity(-1);
        }
        else if(originalSlot.GetItem() == null)
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
            movingSlot.ChangeQuantity(-1);
        }
        else
        {
            
            tempSlot = new SlotTrait(movingSlot);
            movingSlot.AddItem(originalSlot.GetItem(), originalSlot.GetQuantity());
            originalSlot.AddItem(tempSlot.GetItem(),tempSlot.GetQuantity());
            itemCursor.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
        
        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }


        
        //else
        //{
        //    movingSlot = new SlotTrait(originalSlot);
        //    originalSlot.Clear();
        //}

        
        RefreshUI();
        return true;



    }
    private SlotTrait GetClosestSlot()
    {

         float baseWidth = 3840f; 
        float baseHeight = 2160; 


        float scaleFactor = Mathf.Min(Screen.width / baseWidth, Screen.height / baseHeight);


        

       

        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(Input.mousePosition, slots[i].transform.position) < 120 * scaleFactor)
            {
                return Inventory[i];
            }


        }

        return null;
    }

    #endregion Movement

}
