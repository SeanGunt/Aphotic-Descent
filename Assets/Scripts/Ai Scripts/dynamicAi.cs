using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicAi : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField ] private float amount;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;

    [SerializeField] private LayerMask doNotIgnoreLayer;

    private bool unchosen = true;
    private int theChosen = 2;

    // FixedUpdate is called once at the end of a frame
    void FixedUpdate()
    {
        Vector3 centerRay = transform.TransformDirection(new Vector3( 0, 0, 1)) * amount/2;
        Vector3 rightRay = transform.TransformDirection(new Vector3( 1, 0, 1)) * amount;
        Vector3 leftRay = transform.TransformDirection(new Vector3(-1, 0, 1)) * amount;
        
        Debug.DrawRay(transform.position, rightRay, Color.red);
        Debug.DrawRay(transform.position, centerRay, Color.red);
        Debug.DrawRay(transform.position, leftRay, Color.red);

        //
        if((Physics.Raycast(transform.position, rightRay, out hit, amount, doNotIgnoreLayer) || 
        Physics.Raycast(transform.position, leftRay, out hit, amount, doNotIgnoreLayer) ||
        Physics.Raycast(transform.position, centerRay, out hit, amount/2, doNotIgnoreLayer)) == false)
        {
            Debug.Log("moving forward");
            transform.position += transform.forward * speed * Time.deltaTime;
            unchosen = true;
        }
        else
        {
            Debug.Log(hit.collider.gameObject.name + " was hit by raycast");

            if(unchosen)
            {
                theChosen = Random.Range(0,2);
                Debug.Log("theChosen is " + theChosen);
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
    }

    void rRight()
    {
        transform.Rotate(new Vector3(0, 1, 0) * turnSpeed * Time.deltaTime);
        Debug.Log("rotating right");
    }

    void rLeft()
    {
        transform.Rotate(new Vector3(0, -1, 0) * turnSpeed * Time.deltaTime);
        Debug.Log("rotating left");
    }
}
