using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SonarText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sonarText;
    [SerializeField] private GameObject sonarTextObj;

    private void Update()
    {
        if (GameDataHolder.secondDoorOpened)
        {
            sonarTextObj.SetActive(false);
        }
    }

    private void Awake()
    {
        sonarTextObj.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            sonarTextObj.SetActive(true);
            sonarText.text = "Your sonar will ping key objects. Look out for yellow and blue pings, but be wary of red ones.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            sonarTextObj.SetActive(false);
        }
    }
}
