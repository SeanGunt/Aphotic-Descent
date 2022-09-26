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
        //sets the reset time
        timerReset = timerStart;

        thisObject = this.gameObject;

        scaleIncrease = new Vector3(0.02f, 0.02f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {    
        timerStart -= Time.deltaTime;
        if(timerStart > 0)
        {
            thisObject.transform.localScale += scaleIncrease;
        }
        else
        {
            Destroy(gameObject, timerStart);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    /*
    void OnCollisionEnter(Collision other2)
    {
        Debug.Log(other2.gameObject.name);
    }
    */
}
