using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anglerLure : MonoBehaviour
{
    anglerAi angScript;
    // Start is called before the first frame update
    void Start()
    {
        angScript = GameObject.Find("AnglerPhishe").GetComponent<anglerAi>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Knife")
        {
            Debug.Log("jingle jangle");
            angScript.anglerAgent.destination = this.transform.position;
            angScript.isInvestigating = true;
        }
    }
}
