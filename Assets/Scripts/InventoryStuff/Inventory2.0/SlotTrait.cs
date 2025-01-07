using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class SlotTrait 
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;

    public SlotTrait()
    {
        this.item = null;
        this.quantity = 0;
    }

    public SlotTrait(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;   
    }

    public Item GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public void AddQuantity(int _quantity) { quantity += _quantity; }


}
