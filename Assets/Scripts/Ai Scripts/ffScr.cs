using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ffScr : MonoBehaviour
{
    [SerializeField] private NavMeshAgent theAgent;
    [SerializeField] private Transform target;
    [SerializeField] private bool exists;
    [SerializeField] private float agentSpeed;
    [SerializeField] private bool isPlayerBleeding;
    [SerializeField] private float scentRange;
    [SerializeField] private float rangeMultiplier;
    private float bleedRange;
    private float rangeUsed;
    private GameObject player;
    Vector3 destination;

    [SerializeField] private Transform[] points;
    private int destPoints = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();

        theAgent.speed = agentSpeed;

        theAgent.autoBraking = false;

        bleedRange = scentRange * rangeMultiplier;

    }

    // Update is called once per frame
    void Update()
    {

        if(isPlayerBleeding == true)
        {
            rangeUsed = bleedRange;
            float dist = Vector3.Distance(player.transform.position, transform.position);
        }
        else
        {
            rangeUsed = scentRange;
            float dist = Vector3.Distance(player.transform.position, transform.position);
        }

        if (!theAgent.pathPending && theAgent.remainingDistance < 0.1f)
            {
                patrolling();
            }
    }

    void patrolling()
    {
        if (points.Length == 0)
        {
            return;
        }

        theAgent.destination = points[Random.Range(0, destPoints)].position;
    }

    void attacking()
    {

        if(isPlayerBleeding)
        {
            //if(player.distance)
        }
        
    }

    void wasAttacking() //transition from attack to patrol
    {

    }
}
