using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2v2 : MonoBehaviour
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
             if (col.gameObject.GetComponent<PlayerMovement>() != false)
             {
                if (col.gameObject.GetComponent<PlayerMovement>().hasUpgradedSuit == false)
                {
                    audioSource.PlayOneShot(doorSounds[1]);
                    doorController.close = true;
                    doorController.open = false;
                    doorController.canOpen = false;
                    textController.Text = "Suit not cleared for exit.";
                }
                else
                {
                    doorController.open = true;
                    doorController.close = false;
                    textController.Text = "Suit clear for exit.";
                }
             }
         }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerMovement>().hasUpgradedSuit == false)
        {
            doorController.open = true;
            audioSource.PlayOneShot(doorSounds[0]);
        }
    }
}