using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventroyMan : MonoBehaviour
{


    [SerializeField] private GameObject slotHolder ;
    [SerializeField] private Item itemToAdd ;
    [SerializeField] private Item itemToRemove ;
    //later delete the serialize field
    [SerializeField] private GameObject[] slots;

     public List<SlotTrait> Inventory = new List<SlotTrait>();

    TextMeshPro fuck_you= new TextMeshPro();
    private void Start()
    {

       


        slots = new GameObject[slotHolder.transform.childCount];
        for(int i = 0; i < slotHolder.transform.childCount; i++)
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

        RefreshUI();


        AddToInventory(itemToAdd);
        RemoveFromInventory(itemToRemove);



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
            if(slots.Length > Inventory.Count)
            Inventory.Add(new SlotTrait(Item,Item.GetItem().quantity));
            
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
                SlotTrait Trait = new SlotTrait();
                foreach (SlotTrait slot in Inventory)
                {


                    if (slot.GetItem() == Item)
                    {
                        Trait = slot;
                        break;
                    }
                }
                Inventory.Remove(Trait);
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
        foreach(SlotTrait slot in Inventory)
        {
            if (slot.GetItem() == _item)
            {
                return slot;
            }
        }
        return null;
    }
}
