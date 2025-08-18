using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KelpTutorial : MonoBehaviour
{
     [SerializeField] private GameObject kelpTextObj;
      [SerializeField] private TextMeshProUGUI kelpText;
    //[SerializeField] private GameObject[] tutorialKelp;

    /*private void Update()
    {
        {
            kelpText.text = "";
        }   
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            kelpTextObj.SetActive(true);
            kelpText.text = "Use your tools to overcome obstacles! Different objects can be destroyed with either the knife or blacklight.";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            kelpTextObj.SetActive(false);
        }
    }
}
