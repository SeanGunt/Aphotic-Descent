using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialScript : MonoBehaviour
{
    [SerializeField]
    private Text tutorialText;
    void Start()
    {
        tutorialText.text = "";
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "RoomEntry")
            tutorialText.text = "Room entered/exited.";
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "RoomEntry")
            tutorialText.text = "";
    }
}
