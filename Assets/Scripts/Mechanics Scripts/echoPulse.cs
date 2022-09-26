using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoPulse : MonoBehaviour
{
    
    private GameObject thisObject;

    private float timerStart = 1.0f;

    private float timerReset;

    private Vector3 scaleIncrease;

    // Start is called before the first frame update
    void Start()
    {
        //sets the reset time to start time
        timerReset = timerStart;
        
        //sets the reference of this to thisObject
        thisObject = this.gameObject;

        //sets the scale that it's going to be increased by
        scaleIncrease = new Vector3(0.02f, 0.02f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {    
        //starts a countdown
        timerStart -= Time.deltaTime;

        //while the timer is greater than zero...
        if(timerStart > 0)
        {
            //...it scales the object up in size by the scaleIncrease amount
            thisObject.transform.localScale += scaleIncrease;
        }
        else
        {
            //else it just destroys the object
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //When something hits the trigger, it gets revealed in the DebugLog
        //Note: for an object to be detected, it needs a collider
        Debug.Log(other.gameObject.name);
    }

    //this is just here for another detection method if needed
    //does the same thing as the other one
    /*
    void OnCollisionEnter(Collision other2)
    {
        Debug.Log(other2.gameObject.name);
    }
    */
}
