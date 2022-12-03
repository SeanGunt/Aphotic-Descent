using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Manager : MonoBehaviour
{
    private DoorScript2 doorController;
    private UItext textController, buttonTextController;
    [SerializeField]private GameObject doorHinge, backupSwitch;

    private void Start()
    {
        doorController = doorHinge.GetComponent<DoorScript2>();
        textController = doorHinge.GetComponent<UItext>();
        buttonTextController = backupSwitch.GetComponent<UItext>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
         {
             backupSwitch.SetActive(true);
         }
    }

    public void SetDoorOpenable()
    {
        doorController.close = true;
        doorController.canOpen = true;
        textController.Text = "The backup switch worked. Door can open.";
        buttonTextController.Text = "Check the door.";
    }
}
