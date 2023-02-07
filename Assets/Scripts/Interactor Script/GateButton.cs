using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GateButton : MonoBehaviour
{
    public bool inTrigger;
    GateScr gScr;
    private PlayerInputActions playerInputActions;
    private InputAction interact;
 
    void Awake()
    {
        gScr = this.gameObject.GetComponentInChildren<GateScr>();
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = playerInputActions.PlayerControls.Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
 
    void Update()
    {
        if(inTrigger)
        {
            if(interact.triggered)
            {
                if(gScr != null)
                {
                    gScr.gateClosed = true;
                }
            }
        }
    }
}
