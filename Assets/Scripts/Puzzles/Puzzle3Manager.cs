using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Manager : MonoBehaviour
{
    private DoorScript2 doorController;
    private UItext textController, buttonTextController;
    [SerializeField]private GameObject doorHinge, backupSwitch;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorCanOpenSound;

    private void Start()
    {
        doorController = doorHinge.GetComponent<DoorScript2>();
        textController = doorHinge.GetComponent<UItext>();
        buttonTextController = backupSwitch.GetComponent<UItext>();
    }

    public void SetDoorOpenable()
    {
        doorController.close = true;
        doorController.canOpen = true;
        audioSource.PlayOneShot(doorCanOpenSound);
        textController.Text = "The backup switch worked. Door can open.";
        buttonTextController.Text = "Storage room open.";
    }
}
