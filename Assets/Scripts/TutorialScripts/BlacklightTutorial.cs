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
            flashlightText.text = "";
        }
        else if(other.gameObject.tag == "Player" && GameDataHolder.flashlightHasBeenPickedUp)
        {
            flashlightTextObj.SetActive(true);
            flashlightText.text = "Shining the blacklight on purple objects can cause certain effects, try it out on the intercom in front of you";
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
