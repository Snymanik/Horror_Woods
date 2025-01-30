using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public int health;
    //[SerializeField] GameObject item;

    void Start()
    {
        health += Random.Range(-5, 5);
    }


}
//GameObject droppedItem = Instantiate(item, gameObject.transform.position, Quaternion.identity);
//droppedItem.GetComponent<Item>().quantity += Random.Range(-5, 5);