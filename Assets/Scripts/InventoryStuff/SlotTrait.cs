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

    public SlotTrait(SlotTrait slot)
    {
        item = slot.item;
        quantity = slot.quantity;
    }
    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }
    public Item GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public void ChangeQuantity(int _quantity) { quantity += _quantity; }
    public void AddItem(Item item, int _quantity)
    {
        this.item = item;
        this.quantity = _quantity;
    }
}
