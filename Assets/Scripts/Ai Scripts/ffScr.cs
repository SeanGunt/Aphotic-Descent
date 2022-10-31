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
    [SerializeField] private GameObject player;
    private float bleedRange;
    private float rangeUsed;
    Vector3 destination;
    private float distanceBtwn;

    private bool unchosen = true;

    [SerializeField] private Transform[] points;

    

    // Start is called before the first frame update
    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();

        theAgent.speed = agentSpeed;

        //theAgent.autoBraking = false;

        theAgent.angularSpeed = agentSpeed*3;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Debug.Log("player found");
        }
        else
        {
            Debug.Log("player not Found");
        }

        bleedRange = scentRange * rangeMultiplier;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(destination);

        distanceBtwn = (player.transform.position-transform.position).sqrMagnitude;

        if(isPlayerBleeding == true)
        {
            rangeUsed = bleedRange;
        }
        else
        {
            rangeUsed = scentRange;
        }

        if(!theAgent.pathPending && theAgent.remainingDistance < 0.5f)
        {
            patrolling();
            unchosen = true;
        }

        if(distanceBtwn < rangeUsed)
        {
            wasPatrolliong();
        }
        else
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
        
        if(unchosen == true)
        {
            destination = points[Random.Range(0, points.Length)].position;
            unchosen = false;
        }

        theAgent.destination = destination;

        Debug.Log("distance left is " + theAgent.remainingDistance);
        Debug.Log("currently going towards " + theAgent.destination);
        //Debug.Log("is at destination: " + theAgent.pathPending + " and status is " + theAgent.pathStatus);
    }

    void attacking()
    {

        
        
    }

    void wasAttacking() //transition from attack to patrol
    {

    }

    void wasPatrolliong() //transition from patrol to attack
    {
        theAgent.destination = player.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {

    }
}
