using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createPulse : MonoBehaviour
{
    public GameObject theObject;

    private Vector3 thisPosition;

    // Update is called once per frame
    void Update()
    {
        thisPosition = GameObject.Find("shootPoint").transform.position;

        if(Input.GetKeyDown("left shift"))
        {
            Instantiate(theObject, thisPosition, Quaternion.identity);
        }
    }
}