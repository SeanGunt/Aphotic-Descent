using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlacklightTutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI flashlightText;
    [SerializeField] private GameObject flashlightTextObj;

    private void Update()
    {
        if (GameDataHolder.secondDoorOpened)
        {
            flashlightTextObj.SetActive(false);
        }
    }

    private void Awake()
    {
        flashlightTextObj.SetActive(false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "I should pick up the flashlight...";
        }
        else if(other.gameObject.tag == "Player" && GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "Hold R to enable the blacklight while the flashlight is on. Shine the blacklight on highlighted objects.";
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
