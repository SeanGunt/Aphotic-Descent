using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript2 : MonoBehaviour
{
    public bool open;
    public bool close;
    public bool inTrigger;
    public bool canOpen;
    private BoxCollider bCollider;
    private UItext uItext;
    [SerializeField]private float rotX, rotY, rotZ;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpenSound;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        inTrigger = false;
        bCollider = GetComponent<BoxCollider>();
        uItext = GetComponent<UItext>();
        canOpen = true;
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
 
    void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Player") 
         {
             inTrigger = true;
         }
    }
 
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
         {
            inTrigger = false;
         }
    }
 
    void Update()
    {
        if (inTrigger)
        {
            if (close)
            {
                if (GameDataHolder.doorKey && canOpen)
                {
                    if (playerInputActions.PlayerControls.Interact.triggered)
                    {
                        audioSource.PlayOneShot(doorOpenSound);
                        bCollider.enabled = false;
                        inTrigger = false;
                        uItext.GuiOn = false;
                        open = true;
                        close = false;
                    }
                }
            }
        }
 
        if (open)
        {
            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotX, rotY - 90, rotZ), Time.deltaTime * 200);
            transform.rotation = newRot;
        }
        else
        {
            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotX, rotY, rotZ), Time.deltaTime * 200);
            transform.rotation = newRot;
        }
    }
}
