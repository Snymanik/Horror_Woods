using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroy : MonoBehaviour
{

    // DELETE THE WHOLE THING IF IT IS UNNECCESARY LIKE WITH THE SCENES
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectindex;
    void Awake()
    {
        if (persistentObjects[objectindex] == null)
        {
            persistentObjects[objectindex] = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(persistentObjects[objectindex] != this.gameObject)
        {
            Destroy(this.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
