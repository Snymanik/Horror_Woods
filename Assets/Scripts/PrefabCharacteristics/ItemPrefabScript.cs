using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabScript : MonoBehaviour
{
    public int prefabQuantity = 1;
    public InventoryController scriptibleObjectType;
    private void Awake()
    {
        prefabQuantity = Random.Range(1, 11);
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(gameObject.layer,3);

        
    }
    private void Update()
    {
        if (prefabQuantity == 0)
        {
            Destroy(this.gameObject);
        }
    }


}
