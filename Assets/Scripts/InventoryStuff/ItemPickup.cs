using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    public PlayerController playerController;


    #region ItemPicking
    
    RaycastHit Checkahead;

    [SerializeField]
    LayerMask ItemLayer;
    Ray bloedirig;
    Transform Selected;
    #endregion


    //#region Inventory
    //ItemsScriptibleObject item;

    //#endregion

    //public void Start()
    //{
    //   item = FindObjectOfType<ItemsScriptibleObject>();
    //}


    // Update is called once per frame
    void FixedUpdate()
    {
       bloedirig = Camera.main.ScreenPointToRay(Input.mousePosition);
       
          
        
        #region ItemPickup
        if (Physics.Raycast(bloedirig, out Checkahead,5, ItemLayer))
        {

            Selected  = Checkahead.transform;
            Selected.GetComponent<Renderer>().material.color = Color.red;

            
               
            
            
        }
        #endregion
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Selected != null)
        {
            //ItemsScriptibleObject.AddItem(Selected.GetComponent<ItemPrefabScript>().ScriptibleObjectType,Selected.GetComponent<ItemPrefabScript>().prefabQuantity);
           
            

             while(Selected.GetComponent<ItemPrefabScript>().prefabQuantity > 0)
             {

                //int help = Selected.GetComponent<ItemPrefabScript>().prefabQuantity;
                // fix this


                // this.gameObject.GetComponent<ItemsScriptibleObject>().AddItem(Selected.gameObject.GetComponent<ItemPrefabScript>().scriptibleObjectType);
                Debug.Log(Selected.gameObject.GetComponent<ItemPrefabScript>().scriptibleObjectType.itemName);



                if(0 == Selected.GetComponent<ItemPrefabScript>().prefabQuantity)
                 {
                     break;
                 }
                Selected.GetComponent<ItemPrefabScript>().prefabQuantity--;
                Debug.Log("IDK");


             }

            // CHECK THIS OUT
        }
        else
        {
            if (Selected != null)
            {
                StartCoroutine(ItemDeselection(Selected));
            }

        }
    }

    IEnumerator ItemDeselection(Transform selected)
    {
      
        yield return new WaitForSeconds(0.5f);

        
        
        if (Physics.Raycast(bloedirig, out Checkahead, 20, ItemLayer))
        {
            if(Checkahead.transform.position != selected.transform.position) 
            {
                selected.GetComponent<Renderer>().material.color = Color.blue;
            }

        }
        else
        {
            if (selected != null)
            {
                selected.GetComponent<Renderer>().material.color = Color.blue;
            }
           
        }
             
        

        
    }
}
