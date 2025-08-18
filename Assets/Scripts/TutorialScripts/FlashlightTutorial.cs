using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashlightTutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI flashlightText;
    [SerializeField] private GameObject flashlightTextObj;

    

    private void Awake()
    {
        flashlightTextObj.SetActive(false);
    }
    
    /*private void Update()
    {
        if (GameDataHolder.secondDoorOpened)
        {
            flashlightTextObj.SetActive(true);
        }
    }*/
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "";
        }
        else if (other.gameObject.tag == "Player" && GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "F to enable and disable the flashlight";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flashlightTextObj.SetActive(false);
        }
    }
}
