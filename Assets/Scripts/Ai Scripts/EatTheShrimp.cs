using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EatTheShrimp : MonoBehaviour
{
    public GameObject crab;
    public GameObject[] pathPoints;
    [HideInInspector] public bool isMoving;
    public int numberOfPoints;
    public float speed;
    public GameObject yummyShrimp;
    public GameObject shrimpCollider;
    private Vector3 actualPosition;
    private int x;
    NavMeshAgent hermitAgent;

    RigBuilder hermitRig;

    void Awake()
    {
        x = 0;

        hermitAgent = this.gameObject.GetComponent<NavMeshAgent>();
        hermitAgent.speed = speed;

        hermitRig = this.gameObject.GetComponent<RigBuilder>();
        hermitRig.enabled = !hermitRig.enabled;
    }

    private void Update()
    {
        if(isMoving)
        {
            hermitMove();
        }
    }
     void hermitMove()
     {
        /*
        actualPosition = crab.transform.position;
        crab.transform.position = Vector3.MoveTowards(actualPosition, pathPoints[x].transform.position, speed * Time.deltaTime);
        if(actualPosition == pathPoints[x].transform.position&& x != numberOfPoints -1) 
        {
            x++;
        }
        */

        hermitAgent.destination = pathPoints[0].transform.position;
        Debug.Log("hermit start moving");
     }
      
    public void MakeMove()
    {
        hermitRig.enabled = !hermitRig.enabled;
        isMoving = true;
    }
}