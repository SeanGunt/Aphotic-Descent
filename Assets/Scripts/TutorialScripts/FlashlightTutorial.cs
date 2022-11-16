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
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "I probably should pick up the flashlight...";
        }
        else if(other.gameObject.tag == "Player" && GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "F to use flashlight, R to enable the blacklight while the flashlight is on";
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
