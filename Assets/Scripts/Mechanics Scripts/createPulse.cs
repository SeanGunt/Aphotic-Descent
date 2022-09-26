using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createPulse : MonoBehaviour
{
    public GameObject theObject;

    private Vector3 thisPosition;

    public GameObject shootingPosition;

    // Update is called once per frame
    void Update()
    {
        //gets the transform position of the gameObject and converts it into a Vector3, because it doesn't work as a Vector3 on its own
        thisPosition = shootingPosition.transform.position;

        if(Input.GetKeyDown("left shift"))
        {
            //when left shift (button can be changed) is pressed, creates a radar pulse object
            Instantiate(theObject, thisPosition, Quaternion.identity);
        }
    }
}