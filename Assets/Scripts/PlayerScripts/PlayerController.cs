using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Bruh
    [SerializeField]  Item checkthis;
    CharacterController characterController;

    #endregion

    #region Interaction
    Ray interact;
    RaycastHit hit;
    [SerializeField]
    LayerMask function;
    public bool furnaceOpened = false;
    [SerializeField] private GameObject furnacePanel;
    #endregion

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
    GameObject camera_;
    public Vector3 dir;
    [SerializeField]
    float sensitivity;
    #endregion
    #region Jumping
    bool isGrounded;
    float Timing;
    
    [SerializeField]
    float jumpCooldown,length;
    #endregion
    #region PlayerMovement
    Rigidbody rb;
     float Horizontal,Vertical;
    [SerializeField]
    private float speed,drag,maxVelocity;
    #endregion
    #region PlayerInventory
    public bool inventoryOpen = false;
    [SerializeField] private GameObject invPanel;

    #endregion

    #region SoundParameters
    public AudioSource audioSource;
    [SerializeField] private AudioClip[] grassSteps;
    [SerializeField] private AudioClip[] jumpingSound;
    private const float stepInterval = 0.5f;
    private  float timeStamp;
    private const float velocityThreshold = 1f;

    #endregion

    #region Sound
    float nextPlayTime = 0;
    [SerializeField] AudioClip[] soundClips;


    [SerializeField] AudioClip[] growlClip;
    float nextGrowl = 0;
    #endregion
    void Start()
    {
        nextPlayTime = Time.time + UnityEngine.Random.Range(5, 10);
        nextGrowl = Time.time + UnityEngine.Random.Range(200, 400);
        characterController = GetComponent<CharacterController>();
       Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        invPanel.SetActive(false);
        furnacePanel.SetActive(false);
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
        interact = Camera.main.ScreenPointToRay(Input.mousePosition);
        //MouseY = Mathf.Clamp(MouseY, -90, 90);



        xRotation -= MouseY * Time.deltaTime * sensitivity;
        yRotation += MouseX * Time.deltaTime * sensitivity;
        xRotation = Mathf.Clamp(xRotation,-90, 90);
        if (!inventoryOpen)
        {
            camera_.transform.rotation = Quaternion.Euler(xRotation, yRotation + 90, 0);
            this.gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        

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
            speed += 20f;
            //StopCoroutine(SprintingMode(1));
            StopAllCoroutines();
            StartCoroutine(SprintingMode());
            
            
        }
        else if (!isSprinting && !keyProblemSolver || stamina <= 0 && !keyProblemSolver)
        {
            speed -= 20f;
            //StartCoroutine(SprintingMode(1));
            StopAllCoroutines();
            StartCoroutine(SprintingOff());
            keyProblemSolver =true;
        }

        

    }
    private void Update()
    {
        isGrounded = Physics.Raycast(this.gameObject.transform.position, Vector3.down, length * 0.5f);
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddObject(checkthis);
        }
        if (Input.GetKeyDown(KeyCode.I) && !inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryOpen = true;
            invPanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.I))
            {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryOpen = false;
            invPanel.SetActive(false);

            if (furnaceOpened)
            {
                furnaceOpened = false;  
                furnacePanel.SetActive(false);
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            OpenSmth();
        }

        CheckMove();


        if (Time.time >= nextPlayTime)
        {
            PlayRandomSound();
            nextPlayTime = Time.time + UnityEngine.Random.Range(100, 170);
        }
        if (Time.time >= nextGrowl)
        {
            PlayRandomGrowl();
            nextGrowl = Time.time + UnityEngine.Random.Range(200, 350);
        }
    }

    private void PlayRandomGrowl()
    {
        AudioClip clip = growlClip[UnityEngine.Random.Range(0, growlClip.Length)];
        audioSource.PlayOneShot(clip);
    }

    private void PlayRandomSound()
    {
        AudioClip clip = soundClips[UnityEngine.Random.Range(0, soundClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    private void CheckMove()
    {
      
        if (isGrounded && rb.velocity.magnitude > velocityThreshold)
        {
            if (isSprinting)
            {
                timeStamp -= 1.4f*Time.deltaTime;
            }
            else
            {
                timeStamp -= Time.deltaTime;
            }

            if (timeStamp <= 0)
            {
                audioSource.PlayOneShot(grassSteps[UnityEngine.Random.Range(0, grassSteps.Length - 1)]);
                timeStamp = stepInterval;

            }


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
        if (stamina < 20)
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
        if(rb.velocity.magnitude > 7 && characterController.isGrounded)
        {
            SpeedControl = SpeedControl.normalized * 7;
            rb.velocity = new Vector3(SpeedControl.x,rb.velocity.y,SpeedControl.z);
        } 
    }
    private void CheckIfCanJump()
    {



        

        if (isGrounded & Time.fixedTime > Timing)
        { 
            rb.AddForce(Vector3.up * 5,ForceMode.Impulse);

            audioSource.PlayOneShot(jumpingSound[UnityEngine.Random.Range(0, jumpingSound.Length - 1)]);
             

            Vector3 AirControll = new Vector3(rb.velocity.x, 0, rb.velocity.z);   
                AirControll = AirControll.normalized * 0.1f;
            rb.velocity = new Vector3(AirControll.x, rb.velocity.y, AirControll.z);

            Timing = Time.fixedTime +jumpCooldown;
        }
        
     
    }

    //idk what i am doing
    // public int Quantity;
    private void OpenSmth()
    {
        if (Physics.Raycast(interact,out hit, 5,function))
        {
            if(hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Furnace"))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    furnaceOpened = true;
                    inventoryOpen = true;
                    invPanel.SetActive(true);
                    furnacePanel.SetActive(true);

                }
            }
        }
    }
    public void AddObject(Item InvContr)
    {

        if (InvContr.gobject != null)
        {
            GameObject prefabToSpawn = InvContr.gobject;
            
            GameObject spawnedObject = Instantiate(prefabToSpawn, this.gameObject.transform.position, Quaternion.identity);

           
            spawnedObject.GetComponent<ItemPrefabScript>().scriptibleObjectType = InvContr;

           // Quantity = Random.Range(1, InvContr.quantity);


        }
        else
        {
            Debug.LogError("No prefab assigned in the ScriptableObject!");
        }
    }
}
