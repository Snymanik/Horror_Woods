using sc.terrain.vegetationspawner;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ItemPickupDestroy : MonoBehaviour
{
    [SerializeField]
    public PlayerController playerController;
    [SerializeField] private GameObject invManager;


    RaycastHit Checkahead,Treehit;
    Ray bloedirig;

    #region ItemPicking
    [SerializeField]
    LayerMask ItemLayer;
    Transform Selected;
    #endregion


    #region ItemDestroy
    [SerializeField] 
    LayerMask groundLayer;
    float timePassed = 0;
    [SerializeField]
    Item[] Script;

    
    


    #endregion

   

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

           
            
            invManager.GetComponent<InventroyMan>().AddToInventory(Selected.GetComponent<ItemPrefabScript>().scriptibleObjectType, Selected.GetComponent<ItemPrefabScript>().scriptibleObjectType.GetItem().quantity);
            invManager.GetComponent<InventroyMan>().torchLocations.Remove(Selected.gameObject.transform);
            Destroy(Selected.gameObject);

        }else
        {
            if (Selected != null)
            {
                StartCoroutine(ItemDeselection(Selected));
            }
        }
        
       
        if (Input.GetMouseButton(0)) 
        {
            if (Physics.Raycast(bloedirig, out Treehit, 5, groundLayer))
            {
                if(Treehit.collider.gameObject != null)
                {
                  
                    if (Treehit.collider.CompareTag("Tree"))
                    {
                        
                        timePassed += Time.deltaTime;
                        

                        if (timePassed > 25 - invManager.GetComponent<InventroyMan>().toolEfficency)
                        {

                            Destroy(Treehit.collider.gameObject);
                            GameObject Drops = Instantiate(Script[0].GetItem().gobject,this.gameObject.transform.position, Quaternion.identity);
                            Drops.GetComponent<ItemPrefabScript>().scriptibleObjectType = Script[0];

                            timePassed = 0;
                            return;
                            
                        }
                    }
                }
                
            }
            

        }
        else
        {
            timePassed = 0;
        }

    }

    IEnumerator ItemDeselection(Transform selected)
    {
      
        yield return new WaitForSeconds(0.5f);

        
        
        if (Physics.Raycast(bloedirig, out Checkahead, 20, ItemLayer))
        {
            if (selected != null)
            {
                if (Checkahead.transform.position != selected.transform.position)
                {
                    selected.GetComponent<Renderer>().material.color = Color.blue;
                }
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
