using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashlightPickup : MonoBehaviour
{
    public bool inTrigger;
    public GameObject tutTextObj;
    public TextMeshProUGUI tutText;
 
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
            if (Input.GetButtonDown("Interact"))
            {
                tutTextObj.SetActive(true);
                tutText.text = "Flashlight Acquired";
                GameDataHolder.flashlightHasBeenPickedUp = true;
                Destroy(this.gameObject);
            }
        }
    }
}
