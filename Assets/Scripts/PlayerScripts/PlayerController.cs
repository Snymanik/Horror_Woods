using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Bruh
    [SerializeField]  InventoryController checkthis;

    #endregion

    [SerializeField]
    GameObject camera_;
    
    public Vector3 dir;
    
    #region Sprinting
    bool keyProblemSolver = true;
    bool isSprinting;
    [SerializeField]
    int stamina = 10;
    int maxStamina = 10;

    #endregion
    #region CameraMovement
    float MouseX, MouseY,xRotation,yRotation;
    [SerializeField]
    float sensitivity;
    #endregion
    #region Jumping
    bool isGrounded;
    float Timing;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float jumpCooldown,length;
    #endregion
    #region PlayerMovement
    Rigidbody rb;
     float Horizontal,Vertical;
    [SerializeField]
    private float speed,drag,maxVelocity;
    #endregion
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Horizontal = -Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        MouseY = Input.GetAxis("Mouse Y");
        MouseX = Input.GetAxis("Mouse X");
        isSprinting= Input.GetKey(KeyCode.R);
        //MouseY = Mathf.Clamp(MouseY, -90, 90);
        


        xRotation -= MouseY * Time.deltaTime * sensitivity;
        yRotation += MouseX * Time.deltaTime * sensitivity;
        xRotation = Mathf.Clamp(xRotation,-90, 90);
        camera_.transform.rotation = Quaternion.Euler(xRotation, yRotation+90, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);

        dir = this.gameObject.transform.forward*Horizontal  + this.gameObject.transform.right*Vertical ;
        rb.AddForce(dir*speed,ForceMode.Force);
        rb.drag = drag;

        SpeedControl();
        

        if (Input.GetKey(KeyCode.Space))
        {
            CheckIfCanJump();
        }
        
        if (isSprinting && keyProblemSolver && stamina >= 1)
        {
            keyProblemSolver = false;
            speed += 10f;
            //StopCoroutine(SprintingMode(1));
            StopAllCoroutines();
            StartCoroutine(SprintingMode());
            
            
        }
        else if (!isSprinting && !keyProblemSolver || stamina <= 0 && !keyProblemSolver)
        {
            speed -= 10f;
            //StartCoroutine(SprintingMode(1));
            StopAllCoroutines();
            StartCoroutine(SprintingOff());
            keyProblemSolver =true;
        }

        

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // InventoryController Fuckmke = AssetDatabase.LoadAssetAtPath<InventoryController>("Assets / ScriptableObjects / Wood.asset");

            // Debug.Log($"{Fuckmke} p has been pressed");



            AddObject(checkthis);

        }
    }

    IEnumerator SprintingMode()
    {
        yield return new WaitForSeconds(1);
        
            stamina -= 1 ;
        
        
        if (isSprinting)
        {

            StartCoroutine(SprintingMode());
        }
        
    }
    IEnumerator SprintingOff( )
    {
        yield return new WaitForSeconds(3);
        if (stamina < 10)
        {
            stamina += 1;
        }

        if (!isSprinting)
        {

            StartCoroutine(SprintingOff());
        }

    }

    private void SpeedControl()
    {
         Vector3 SpeedControl = new Vector3(rb.velocity.x,0,rb.velocity.z);
        if(rb.velocity.magnitude > 7 && isGrounded)
        {
            SpeedControl = SpeedControl.normalized * 7;
            rb.velocity = new Vector3(SpeedControl.x,rb.velocity.y,SpeedControl.z);
        }
        
            
        
    }
    private void CheckIfCanJump()
    {
        
        isGrounded = Physics.Raycast(this.gameObject.transform.position, Vector3.down, length*0.4f, groundLayer);
       
        if (isGrounded)
        {
            

        }
            
        if(isGrounded && Time.fixedTime > Timing)
        { 
            rb.AddForce(Vector3.up * 5,ForceMode.Impulse);

            Vector3 AirControll = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                
                AirControll = AirControll.normalized * 0.1f;
            rb.velocity = new Vector3(AirControll.x, rb.velocity.y, AirControll.z);

            Timing = Time.fixedTime +jumpCooldown;
        }
     
    }

    //idk what i am doing
   // public int Quantity;

    public void AddObject(InventoryController InvContr)
    {

        if (InvContr.gobject != null)
        {

            GameObject spawnedObject = Instantiate(InvContr.gobject, this.gameObject.transform.position, Quaternion.identity);
            spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType = InvContr;

           // Quantity = Random.Range(1, InvContr.quantity);


        }
        else
        {
            Debug.LogError("No prefab assigned in the ScriptableObject!");
        }
    }
}
