using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
 
public class DoorScript : MonoBehaviour 
{
 
    public bool open;
    public bool close;
    public bool inTrigger;
    private BoxCollider bCollider;
    private UItext uItext;
    private PlayerInputActions playerInputActions;
    private InputAction interact;

    private void Awake()
    {
        inTrigger = false;
        bCollider = GetComponent<BoxCollider>();
        uItext = GetComponent<UItext>();
        playerInputActions = InputManager.inputActions;
    }
    
    private void OnEnable()
    {
        interact = playerInputActions.PlayerControls.Interact;
        //playerInputActions.Enable();
    }

    private void OnDisable()
    {
        //playerInputActions.Disable();
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
                if (GameDataHolder.doorKey)
                {
                    if (interact.triggered)
                    {
                        bCollider.enabled = false;
                        uItext.GuiOn = false;
                        open = true;
                        close = false;
                    }
                }
            }
        }
 
        if (open)
        {
            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), Time.deltaTime * 200);
            transform.rotation = newRot;
        }
        else
        {
            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), Time.deltaTime * 200);
            transform.rotation = newRot;
        }
    }
}
