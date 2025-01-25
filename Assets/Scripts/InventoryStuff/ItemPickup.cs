using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    public PlayerController playerController;
    [SerializeField] private GameObject invManager;

    #region ItemPicking

    RaycastHit Checkahead;

    [SerializeField]
    LayerMask ItemLayer;
    Ray bloedirig;
    Transform Selected;
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
           // Debug.Log(Selected.GetComponent<ItemPrefabScript>().scriptibleObjectType + "  " + Selected.GetComponent<ItemPrefabScript>().scriptibleObjectType.GetItem().quantity);
            invManager.GetComponent<InventroyMan>().AddToInventory(Selected.GetComponent<ItemPrefabScript>().scriptibleObjectType, Selected.GetComponent<ItemPrefabScript>().scriptibleObjectType.GetItem().quantity);
            Destroy(Selected.gameObject);

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
