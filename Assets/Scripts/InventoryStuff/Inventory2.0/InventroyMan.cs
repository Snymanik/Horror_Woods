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
    private GameObject[] slots;

    public List<SlotTrait> Inventory = new List<SlotTrait>();

    TextMeshPro fuck_you= new TextMeshPro();
    private void Start()
    {

       


        slots = new GameObject[slotHolder.transform.childCount];
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
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Inventory[i].GetQuantity().ToString();
                 
            }
            catch
                {
                    slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = null;
                    slots[i].transform.GetChild(0).GetComponent<RawImage>().enabled = false;
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                  
                    


            }

            
        }

    }
    public void AddToInventory(Item Item) {

        //Inventory.Add(Item);
        RefreshUI();
    }
    
    public void RemoveFromInventory(Item Item) {
       // Inventory.Remove(Item);
        RefreshUI();
    }
}
