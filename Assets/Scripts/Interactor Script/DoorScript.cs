using UnityEngine;
using System.Collections;
 
public class DoorScript : MonoBehaviour {
 
    public static bool doorKey;
    public bool open;
    public bool close;
    public bool inTrigger;

    private void Awake()
    {
        inTrigger = false;
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
        inTrigger = false;
    }
 
    void Update()
    {
        if (inTrigger)
        {
            if (close)
            {
                if (doorKey)
                {
                    if (Input.GetButtonDown("Interact"))
                    {
                        open = true;
                        close = false;
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Interact"))
                {
                    close = true;
                    open = false;
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
