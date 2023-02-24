using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Manager : MonoBehaviour
{
    public bool barnacleTrig1, barnacleTrig2, barnacleTrig3, barnacleTrig4, barnacleTrig5;
    private DoorScript3 doorController;
    private UItext textController;

    [SerializeField] private GameObject doorControl;

    private void Start()
    {
        //doorController = doorHinge.GetComponent<DoorScript2>();
        doorController = doorControl.GetComponent<DoorScript3>();
        textController = doorControl.GetComponent<UItext>();
    }
    
    private void Update()
    {
        if (barnacleTrig1 && barnacleTrig2 && barnacleTrig3 && barnacleTrig4 && barnacleTrig5)
        {
            doorController.canOpen = true;
            doorController.close = true;
            textController.Text = "Mechanism fixed. Door unlocked.";
        }
    }

    public void ActivateTrig1()
    {
        barnacleTrig1 = true;
        Debug.Log("Trigger1Hit");
    }

    public void ActivateTrig2()
    {
        barnacleTrig2 = true;
        Debug.Log("Trigger2Hit");
    }

    public void ActivateTrig3()
    {
        barnacleTrig3 = true;
        Debug.Log("Trigger3Hit");
    }

    public void ActivateTrig4()
    {
        barnacleTrig4 = true;
        Debug.Log("Trigger4Hit");
    }

    public void ActivateTrig5()
    {
        barnacleTrig5 = true;
        Debug.Log("Trigger5Hit");
    }
}
