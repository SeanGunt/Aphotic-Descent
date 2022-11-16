using UnityEngine;
using System.Collections;
using TMPro;
 
public class DoorKey : MonoBehaviour 
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
                tutText.text = "Knife Acquired";
                GameDataHolder.knifeHasBeenPickedUp = true;
                DoorScript.doorKey = true;
                Destroy(this.gameObject);
            }
        }
    }
}