using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    public bool inTrigger;
    GateScr gScr;
 
    void Awake()
    {
        gScr = this.gameObject.GetComponentInChildren<GateScr>();
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
            if(Input.GetButtonDown("Interact"))
            {
                if(gScr != null)
                {
                    gScr.gateClosed = true;
                }
            }
        }
    }
}
