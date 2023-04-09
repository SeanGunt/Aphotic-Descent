using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EatTheShrimp : MonoBehaviour
{
    public GameObject[] pathPoints;
    [HideInInspector] public bool isMoving;
    public float speed;
    NavMeshAgent hermitAgent;

    RigBuilder hermitRig;

    void Awake()
    {

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
        hermitAgent.destination = pathPoints[0].transform.position;
     }
      
    public void MakeMove()
    {
        hermitRig.enabled = !hermitRig.enabled;
        isMoving = true;
    }
}