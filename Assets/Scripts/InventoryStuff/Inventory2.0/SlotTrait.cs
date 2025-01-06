using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTrait 
{
    private Item item;
    private int quantity;



    public SlotTrait(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;   
    }

    public Item GetItem() { return item; }
    public int GetQuantity() { return quantity; }


}
