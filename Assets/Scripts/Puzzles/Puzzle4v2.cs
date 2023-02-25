using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4v2 : MonoBehaviour
{
    private DoorScript3 doorController;
    private Puzzle4UI puzzleController;
    private UItext textController, computerTextController;
    [SerializeField]private GameObject doorSlider, puzzleComputer, puzzleUI, blacklightComputer, regularComputer, interactableParticle;

    private void Start()
    {
        doorController = doorSlider.GetComponent<DoorScript3>();
        textController = doorSlider.GetComponent<UItext>();
        puzzleController = puzzleUI.GetComponent<Puzzle4UI>();
        computerTextController = puzzleComputer.GetComponent<UItext>();
        doorController.BreakDoor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
         {
            interactableParticle.SetActive(true);
            regularComputer.SetActive(false);
            blacklightComputer.SetActive(true);
         }
    }

    private void Update()
    {
        if (puzzleController.codeSolved == true)
        {
            SetDoorOpenable();
            doorController.unbroken = true;
        }
    }

    public void SetDoorOpenable()
    {
        textController.Text = "Code solved. Door Fixed";
        computerTextController.Text = "Check the door.";
    }
}
