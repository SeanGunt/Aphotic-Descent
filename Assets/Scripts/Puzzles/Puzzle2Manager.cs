using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2Manager : MonoBehaviour
{
    private DoorScript2 doorController;
    private UItext textController;
    [SerializeField]private GameObject doorHinge;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] doorSounds;

    private void Start()
    {
        doorController = doorHinge.GetComponent<DoorScript2>();
        textController = doorHinge.GetComponent<UItext>();
    }
    private void OnTriggerEnter(Collider col)
    {
         if (col.gameObject.tag == "Player") 
         {
             if (col.gameObject.GetComponent<InvisibilityMechanic>() != false)
             {
                if (col.gameObject.GetComponent<InvisibilityMechanic>().isSafe == false)
                {
                    audioSource.PlayOneShot(doorSounds[1]);
                    doorController.close = true;
                    doorController.open = false;
                    doorController.canOpen = false;
                    textController.Text = "You were seen by the computer.";
                }
                else
                {
                    doorController.open = true;
                    doorController.close = false;
                    textController.Text = "You avoided the computers.";
                }
             }
         }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorController.open = true;
            audioSource.PlayOneShot(doorSounds[0]);
        }
    }
}
