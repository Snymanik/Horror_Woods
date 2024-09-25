using System;
using System.Collections;
using System.Collections.Generic;
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
    #endregion




    // Update is called once per frame
    void FixedUpdate()
    {
       bloedirig = Camera.main.ScreenPointToRay(Input.mousePosition);
       
          
        
        #region ItemPickup
        if (Physics.Raycast(bloedirig, out Checkahead,20, ItemLayer))
        {

            var Selected  = Checkahead.transform;
            Selected.GetComponent<Renderer>().material.color = Color.red;
            if (Input.GetKeyDown(KeyCode.E))
            {
               Destroy(Selected);
                
            }
            StartCoroutine(ItemDeselection(Selected));
        }
        #endregion
    }

    IEnumerator ItemDeselection(Transform selected)
    {

        yield return new WaitForSeconds(0.1f);

        if(!Physics.Raycast(bloedirig, out Checkahead, 20, ItemLayer))
        {
            selected.GetComponent<Renderer>().material.color = Color.blue;
        }

        
    }
}
