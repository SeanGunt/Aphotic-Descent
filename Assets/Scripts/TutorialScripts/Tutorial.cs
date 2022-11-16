using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI boxText;
    [SerializeField] private GameObject boxTextObj;
    [SerializeField] private GameObject tutTextObj;

    private void Update()
    {
        if (GameDataHolder.knifeHasBeenPickedUp)
        {
            boxText.text = "Left Mouse To Swing Knife";
        }

        if (tutTextObj.activeInHierarchy)
        {
            Invoke("ClearUI", 2f);
        }
        else
        {
            CancelInvoke();
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

    private void ClearUI()
    {
        tutTextObj.SetActive(false);
    }
}
