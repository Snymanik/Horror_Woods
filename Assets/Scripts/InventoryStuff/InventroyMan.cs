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
    [SerializeField] private Transform player;
    
    

    #region Hotbar

    [SerializeField] GameObject itemHeld;
    
    [SerializeField] GameObject torch;
    #endregion

    
    float temperature = 90;
    public List<Transform> torchLocations = new List<Transform>();
    [SerializeField] RawImage thermometer;
    [SerializeField] Text tempNum;
    [SerializeField] Texture[] icons = new Texture[3];
    bool tempChecker;
    [SerializeField] RawImage FreezeEffect;
    bool freezing = false;
    private Coroutine activeTempCoroutine = null;
    private Coroutine freezingTransCouroutine = null;



    [SerializeField] private GameObject slotHolder ;
    [SerializeField] private Item itemToAdd ;
    [SerializeField] private Item itemToRemove ;
    private GameObject[] slots;

    [SerializeField] GameObject furnaceSlot;
    [SerializeField] FurnaceController furnaceController;
    

    [SerializeField] private SlotTrait[] startingItems;
    private SlotTrait[] Inventory;

    private SlotTrait movingSlot;
    private SlotTrait tempSlot;
    private SlotTrait originalSlot;
    bool isMovingItem;

    public int toolEfficency = 0;

    const float baseWidth = 3840f;
    const float baseHeight = 2160;
    float scaleFactor = Mathf.Min(Screen.width / baseWidth, Screen.height / baseHeight);
    private void Start()
    {
        activeTempCoroutine = StartCoroutine(TempDecrease());
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



        torch.SetActive(false);

        
    }
    private void Update()
    {
        //Add the hotbar shit
        TemperaturCheck();
        ThermometeController();

        
        
        



        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if(isMovingItem )
        {
            itemCursor.GetComponent<RawImage>().texture = movingSlot.GetItem().Icon;
            if (movingSlot.GetItem().IsStackable)
            {
                itemCursor.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingSlot.GetQuantity().ToString();
            }
            else
            {
                itemCursor.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
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
        if (Input.GetKeyDown(KeyCode.Q) && player.gameObject.GetComponent<PlayerController>().inventoryOpen)
        {
           RemoveFromInventory( GetClosestSlot());

        }
        if(temperature < 0)
        {
            Debug.Log("Ur ded fam");
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
        if (Inventory[15].GetItem() != null)
        {
            if (Inventory[15].GetItem().itemName == "Torch")
            {
                torch.SetActive(true);
                
                itemHeld.GetComponent<MeshFilter>().mesh = null;
            }
            else
            {
                itemHeld.GetComponent<MeshRenderer>().materials = Inventory[15].GetItem().gobject.GetComponent<MeshRenderer>().sharedMaterials;
                itemHeld.GetComponent<MeshFilter>().mesh = Inventory[15].GetItem().gobject.GetComponent<MeshFilter>().sharedMesh;
                torch.SetActive(false);
            }
            if (!Inventory[15].GetItem().IsStackable)
            {
                itemHeld.transform.localScale = new Vector3(2, 2, 2);
                itemHeld.transform.localEulerAngles = new Vector3(0, -90, 90);
                if (Inventory[15].GetItem().itemName == "Chainsaw")
                {
                    itemHeld.transform.localEulerAngles = new Vector3(-90, 0, 90);
                }
                
            }
            else
            {
                itemHeld.transform.localScale = new Vector3(4, 4, 4);
                itemHeld.transform.localEulerAngles = new Vector3(0, -90, 90); 
            }
            if (Inventory[15].GetItem().GetTool() != null)
            {
                Tool tool = Inventory[15].GetItem().GetTool() as Tool;
                toolEfficency = (int)tool.ToolEff;
                
                
            }
        }
        else
        {
            itemHeld.GetComponent<MeshFilter>().mesh = null;
            torch.SetActive(false);
            toolEfficency = 0;
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
                        // break ;
                        RefreshUI();
                        return;
                    }
                    
                    
                }
                             
                    GameObject spawnedObject = Instantiate(Item.GetItem().gobject, player.position, Quaternion.identity);
                    spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType = Item;
                    spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType.GetItem().quantity = quantity;
                    if (Item.GetItem().itemName == "Torch")
                    {
                        torchLocations.Add(spawnedObject.transform);
                    }

            }
            RefreshUI();
        }
       
        
    }
    
    public void RemoveFromInventory(SlotTrait Item) {
        
        //SlotTrait temp = Contains(Item);
        if (Item != null)
        {
           
                RefreshUI();
                GameObject spawnedObject = Instantiate(Item.GetItem().gobject, player.position, Quaternion.identity);
                spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType = Item.GetItem();
                spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType.GetItem().quantity = Item.GetQuantity();
                Item.Clear();
            
            
        }
        else
        {
            Debug.Log("No item in inventory");
        }
        RefreshUI();
    }
    
    public SlotTrait Contains(Item _item) 
    {
        

        for (int i = 0; i < Inventory.Length; i++)
        {
            
            if (Inventory[i].GetItem() == _item && Inventory[i].GetItem().IsStackable && Inventory[i].GetQuantity()+_item.GetItem().quantity <= Inventory[i].GetItem().maxStackSize)
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
            if (FurnaceUI() && movingSlot.GetItem().itemName == "Wood")
            {
                movingSlot = furnaceController.AddFuel(movingSlot);

                if(movingSlot != null)
                {
                    AddToInventory(movingSlot.GetItem(), movingSlot.GetQuantity());

                }
                RefreshUI();
                isMovingItem = false;
                return true;
            }
            // AddToInventory(movingSlot.GetItem(), movingSlot.GetQuantity());  DEPENDS IF YOU WANT IT VBAC
            GameObject spawnedObject = Instantiate(movingSlot.GetItem().gobject, player.position, Quaternion.identity);
             spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType = movingSlot.GetItem();
            spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType.GetItem().quantity = movingSlot.GetQuantity();
            if(movingSlot.GetItem().itemName == "Torch")
            {
                torchLocations.Add(spawnedObject.transform);
            }
            
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

        
        RefreshUI();
        return true;
    }
    private SlotTrait GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(Input.mousePosition, slots[i].transform.position) < 120 * scaleFactor)
            {
                return Inventory[i];
            }

        }

        return null;
    }
    private bool FurnaceUI()
    {
        if(Vector2.Distance(Input.mousePosition,furnaceSlot.transform.position) < 120 * scaleFactor)
        {
            return true;
        }
        return false;
    }

    #endregion Movement

    #region TemperatureLogic
    private IEnumerator TempIncrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (temperature < 90)
            {
                temperature++;
            }
        }    

        
    }
    private IEnumerator TempDecrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(12f);
            temperature--;
        }
            
            
        
    }
    private void TemperaturCheck()
    {
        
            if (Inventory[15].GetItem() != null && Inventory[15].GetItem().itemName == "Torch")
            {
                if (!tempChecker)
                {
                StopActiveCoroutine();
                activeTempCoroutine = StartCoroutine(TempIncrease());
                    
                    tempChecker = true;
                }

            }
            else
            {
                foreach (Transform t in torchLocations)
                {

                    if (Vector3.Distance(player.position, t.position) < 10)
                    {
                        if (!tempChecker)
                        {

                        StopActiveCoroutine();
                        activeTempCoroutine = StartCoroutine(TempIncrease());
                            
                            tempChecker = true;
                        }
                        
                        return;

                    }
                    
                
                }
                    if (tempChecker)
                    {
                    StopActiveCoroutine();
                    activeTempCoroutine = StartCoroutine(TempDecrease());
                        

                        tempChecker = false;
                    }
            }
            
        
        
       
        
    }
    private void StopActiveCoroutine()
    {
        if (activeTempCoroutine != null)
        {
            StopCoroutine(activeTempCoroutine);
            activeTempCoroutine = null;
        }
    }

    private void ThermometeController()
    {
        
        tempNum.text = (Mathf.Round((temperature / 30 + 35)*10)/10).ToString();
        if(temperature >= 60)
        {
         thermometer.texture = icons[0];
        }
        else if(temperature < 60 &&  temperature > 30)
        {
         thermometer.texture = icons[1];
            if (freezing)
            {
                StopFreezeTrans();
                freezingTransCouroutine = StartCoroutine(DefrostingTransition());

                freezing = false;
            }

        }
        else
        {
            
            thermometer.texture = icons[2];
            if (!freezing) 
            {
                StopFreezeTrans();
                freezingTransCouroutine = StartCoroutine(FreezingTransition());
                freezing = true;
            }
        }
        
    }
    private void StopFreezeTrans()
    {
        if (freezingTransCouroutine != null)
        {
            StopCoroutine(freezingTransCouroutine);
            freezingTransCouroutine = null;
        }
    }
    private IEnumerator FreezingTransition()
    {
        
        Color Fcolor = FreezeEffect.color;
        while(Fcolor.a < 1)
        {
            
                yield return new WaitForSeconds(0.05f);
                Fcolor.a += Time.deltaTime;
                FreezeEffect.color = Fcolor;
            
            

        }
        
    }
    private IEnumerator DefrostingTransition()
    {

        Color Fcolor = FreezeEffect.color;
        while (Fcolor.a > 0)
        {
            
                yield return new WaitForSeconds(0.05f);
                Fcolor.a -= Time.deltaTime;
                FreezeEffect.color = Fcolor;
                
            
            
        }
        
    }


    #endregion
}
