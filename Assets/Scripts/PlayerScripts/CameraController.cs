using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject cameraPos;
    void FixedUpdate()
    {
        this.gameObject.transform.position = cameraPos.transform.position;
    }
}
