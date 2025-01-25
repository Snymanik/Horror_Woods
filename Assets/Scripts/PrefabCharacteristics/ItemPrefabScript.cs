using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabScript : MonoBehaviour
{
    //public int prefabQuantity = 1;
    public Item scriptibleObjectType;
   
    
    // Start is called before the first frame update
    void Start()
    {
      
        Physics.IgnoreLayerCollision(gameObject.layer,3);

        
    }
    private void Update()
    {
        if (scriptibleObjectType.GetItem().quantity == 0)
        {
            Destroy(this.gameObject);
        }
    }


}
