using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlacer : MonoBehaviour
{
    [SerializeField]
    Terrain terrain;
    [SerializeField]
    GameObject prefab;
    void Awake()
    {
      
            foreach(TreeInstance tree in terrain.terrainData.treeInstances)
        {
            Vector3 worldTreePos = Vector3.Scale(tree.position, terrain.terrainData.size) + Terrain.activeTerrain.transform.position;
            Instantiate(prefab, worldTreePos,Quaternion.identity);
           
        }
            List<TreeInstance> treeInstances = new List<TreeInstance>();
        terrain.terrainData.treeInstances = treeInstances.ToArray();
    }


}
