using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4Manager : MonoBehaviour
{
    private DoorScript2 doorController;
    private Puzzle4UI puzzleController;
    private UItext textController, computerTextController;
    [SerializeField]private GameObject doorHinge, puzzleComputer, puzzleUI, blacklightComputer, regularComputer, interactableParticle;

    private void Start()
    {
        doorController = doorHinge.GetComponent<DoorScript2>();
        textController = doorHinge.GetComponent<UItext>();
        puzzleController = puzzleUI.GetComponent<Puzzle4UI>();
        computerTextController = puzzleComputer.GetComponent<UItext>();
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
        }
    }

    public void SetDoorOpenable()
    {
        doorController.close = true;
        doorController.canOpen = true;
        textController.Text = "Code solved. E to interact";
        computerTextController.Text = "Check the door.";
    }
}
