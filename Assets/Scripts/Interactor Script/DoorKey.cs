using UnityEngine;
using System.Collections;
 
public class DoorKey : MonoBehaviour {
 
    public bool inTrigger;
 
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
        if (inTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                DoorScript.doorKey = true;
                Destroy(this.gameObject);
            }
        }
    }
 
    
}