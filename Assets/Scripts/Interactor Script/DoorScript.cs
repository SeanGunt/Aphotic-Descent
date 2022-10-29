using UnityEngine;
using System.Collections;
 
public class DoorScript : MonoBehaviour {
 
    public static bool doorKey;
    public bool open;
    public bool close;
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
 
    void OnGUI()
    {
        if (inTrigger)
        {
            if (open)
            {
                GUI.Box(new Rect(1920/2, 1080/2, 200, 25), "Press E to close");
            }
            else
            {
                if (doorKey)
                {
                    GUI.Box(new Rect(1920/2, 1080/2, 200, 25), "Press E to open");
                }
                else
                {
                    GUI.Box(new Rect(1920/2, 1080/2, 200, 25), "Need a key!");
                }
            }
        }
    }
}
