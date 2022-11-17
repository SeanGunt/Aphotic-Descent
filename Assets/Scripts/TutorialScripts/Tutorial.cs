using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI boxText;
    [SerializeField] private GameObject boxTextObj;
    [SerializeField] private GameObject tutTextObj;
    [SerializeField] private GameObject boxes;

    private void Start()
    {
        if (GameDataHolder.boxes == 0)
        {
            boxes.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameDataHolder.knifeHasBeenPickedUp)
        {
            boxText.text = "Left Mouse To Swing Knife";
        }

        if (GameDataHolder.boxes <= 0)
        {
            DestroyBoxText();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxTextObj.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxTextObj.SetActive(false);
        }
    }

    private void DestroyBoxText()
    {
        boxText.text = "";
    }
}
