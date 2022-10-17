using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicAi2 : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField ] private float visionLength;
    [SerializeField] private float centerLength;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private LayerMask doNotIgnoreLayer;

    private bool unchosen = true;
    private bool unchosen2 = true;
    private int theChosen = 2;
    private int theChosen2 = 2;

    // FixedUpdate is called once at the end of a frame
    void FixedUpdate()
    {
        Vector3 centerRay = transform.TransformDirection(new Vector3( 0, 0, 1)) * centerLength;
        Vector3 rightRay = transform.TransformDirection(new Vector3( 1, 0, 1)) * visionLength;
        Vector3 leftRay = transform.TransformDirection(new Vector3(-1, 0, 1)) * visionLength;
        Vector3 upRay = transform.TransformDirection(new Vector3(0, 1, 1)) * visionLength;
        Vector3 downRay = transform.TransformDirection(new Vector3(0, -1, 1)) * visionLength;

        Debug.DrawRay(transform.position, rightRay, Color.red);
        Debug.DrawRay(transform.position, centerRay, Color.red);
        Debug.DrawRay(transform.position, leftRay, Color.red);
        Debug.DrawRay(transform.position, upRay, Color.red);
        Debug.DrawRay(transform.position, downRay, Color.red);

        //if clipping through walls comment out below line
        transform.position += transform.forward * speed * Time.deltaTime;

        if((Physics.Raycast(transform.position, rightRay, out hit, visionLength, doNotIgnoreLayer) || 
        Physics.Raycast(transform.position, leftRay, out hit, visionLength, doNotIgnoreLayer) ||
        Physics.Raycast(transform.position, centerRay, out hit, centerLength, doNotIgnoreLayer)) == false)
        {
            //Debug.Log("moving forward");

            //if clipping through walls un-comment out below line
            //transform.position += transform.forward * speed * Time.deltaTime;
            unchosen = true;
        }
        else
        {
            Debug.Log(hit.collider.gameObject.name + " was hit by raycast");

            if(unchosen)
            {
                theChosen = Random.Range(0,2);
                //Debug.Log("theChosen is " + theChosen);
                unchosen = false;
            }
            else
            {
                if(theChosen == 0)
                {
                    rLeft();
                }
                else if(theChosen == 1)
                {
                    rRight();
                }
            }
        }

        if((Physics.Raycast(transform.position, upRay, out hit, visionLength, doNotIgnoreLayer) ||
        Physics.Raycast(transform.position, downRay, out hit, visionLength, doNotIgnoreLayer)) == false)
        {
            unchosen2 = true;
        }
        else
        {
            if(unchosen2)
            {
                theChosen2 = Random.Range(0,2);
                //Debug.Log("theChosen is " + theChosen2);
                unchosen2 = false;
            }
            else
            {
                if(theChosen2 == 0)
                {
                    rUp();
                }
                else if(theChosen2 == 1)
                {
                    rDown();
                }
            }
        }
    }

    void rRight()
    {
        transform.Rotate(new Vector3(0, 1, 0) * turnSpeed * Time.deltaTime);
        //Debug.Log("rotating right");
    }

    void rLeft()
    {
        transform.Rotate(new Vector3(0, -1, 0) * turnSpeed * Time.deltaTime);
        //Debug.Log("rotating left");
    }
    void rUp()
    {
        transform.Rotate(new Vector3(1, 0, 0) * turnSpeed * Time.deltaTime);
        //Debug.Log("rotating up");
    }

    void rDown()
    {
        transform.Rotate(new Vector3(-1, 0, 0) * turnSpeed * Time.deltaTime);
        //Debug.Log("rotating down");
    }
}