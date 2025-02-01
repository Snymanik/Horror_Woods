using sc.terrain.vegetationspawner;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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
    //ItemBehaviour SelDestroy;
    public Terrain terrain;
    int tree;
    float timePassed = 0;
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

        }else
        {
            if (Selected != null)
            {
                StartCoroutine(ItemDeselection(Selected));
            }
        }
        
       /* if (Input.GetMouseButton(0))
        {
           
            if (Physics.Raycast(bloedirig, out Treehit, 5, groundLayer))
            {
                if (Treehit.collider.name != Terrain.activeTerrain.name)
                {
                    
                    return;
                }

                if (Treehit.transform.gameObject != null)
                {
                    timePassed += Time.deltaTime;
                    if (timePassed > 1)
                    {
                        timePassed = Time.deltaTime;


                        terrain.RemoveTreesAtPosition(Treehit.transform.position,10);
                       


                        Debug.Log(Treehit.transform.gameObject.name);
                        Debug.Log($"Tree at index {tree} destroyed.{Treehit.transform.position} and {GetTree(Treehit.transform.position)}");

                        timePassed = 0f;
                    }
                }
                    
                }
            }else
                {
                    tree = -1;
                    timePassed = 0;
                }*/
        if (Input.GetMouseButtonDown(0)) 
        {
            

            if (Physics.Raycast(bloedirig, out Treehit))
            {
             
                TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
                for (int i = 0; i < terrain.terrainData.treeInstances.Length; i++)
                {
                    TreeInstance treeInstance = terrain.terrainData.treeInstances[i];
                    Vector3 treePosition = Vector3.Scale(treeInstance.position, terrain.terrainData.size) + terrain.transform.position;

                   
                    if (Vector3.Distance(Treehit.point, treePosition) < 10.0f) 
                    {
                        
                        RemoveTree(i);
                        break;
                    }
                }
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


    void RemoveTree(int index)
    {
        // Create a new list of tree instances
        List<TreeInstance> treeInstances = new List<TreeInstance>(terrain.terrainData.treeInstances);
        treeInstances.RemoveAt(index); // Remove the tree at the specified index

        // Update the terrain's tree instances
        terrain.terrainData.treeInstances = treeInstances.ToArray();
    }
    private Vector3 GetTree(Vector3 hitPoint)
    {
        //TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
        TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

       
        
            for(int i = 0; i < treeInstances.Length; i++)
            {
            Vector3 treePosition = treeInstances[i].position;
            // Convert the tree position to world space
            treePosition.x *= terrain.terrainData.size.x;
            treePosition.z *= terrain.terrainData.size.z;
            treePosition += terrain.transform.position;

            if (Vector3.Distance(hitPoint, treePosition) < 50.0f) 
            {
               return treePosition;
            }
        }
            
        

        return new Vector3(0,-100,0);
    }

   
}
