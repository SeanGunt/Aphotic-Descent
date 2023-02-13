using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GateButton : MonoBehaviour
{
    public bool inTrigger;
    GateScr gScr;
    private GameObject Player;
    private PlayerInput playerInput;
 
    void Awake()
    {
        gScr = this.gameObject.GetComponentInChildren<GateScr>();
        Player = GameObject.FindWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
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
            if(playerInput.actions["Interact"].triggered)
            {
                if(gScr != null)
                {
                    gScr.gateClosed = true;
                }
            }
        }
    }
}
