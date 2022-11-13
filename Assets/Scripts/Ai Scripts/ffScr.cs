using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ffScr : MonoBehaviour
{
    private NavMeshAgent theAgent;
    [SerializeField] private float agentSpeed;
    [SerializeField] private float scentRange;
    [SerializeField] private float rangeForBleedMultiplier;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] points;
    private float bleedRange;
    private float rangeUsed;
    Vector3 destination;
    private float playerDistance;
    private bool unchosen = true;
    PlayerHealthController pHC;
    private bool currentlyAttacking = false;

    
    void Awake()
    {
        Debug.developerConsoleVisible = true;
        theAgent = GetComponent<NavMeshAgent>();

        theAgent.speed = agentSpeed;

        theAgent.updateRotation = true;

        //theAgent.autoBraking = false;

        theAgent.acceleration = 250;

        theAgent.angularSpeed = 250;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Debug.LogWarning("player found");
            pHC = player.GetComponent<PlayerHealthController>();
        }
        else
        {
            Debug.LogWarning("player not Found");
        }

        bleedRange = scentRange * rangeForBleedMultiplier;

    }

    // Update is called once per frame
    void Update()
    {     

        playerDistance = (player.transform.position-this.transform.position).sqrMagnitude;

        if(pHC.isBleeding)
        {
            rangeUsed = bleedRange;
        }
        else
        {
            rangeUsed = scentRange;
        }

        if((!theAgent.pathPending && theAgent.remainingDistance < 0.5f) && !currentlyAttacking)
        {
            unchosen = true;
            patrolling();
        }

        if(playerDistance < rangeUsed*rangeUsed)
        {
            Debug.Log("within attack range");
            theAgent.destination = player.transform.position;
            attacking();
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
        //Debug.Log("currently going towards " + theAgent.destination);
        //Debug.Log("is at destination: " + theAgent.pathPending + " and status is " + theAgent.pathStatus);
    }

    void wasAttacking() //transition from attack to patrol
    {
        currentlyAttacking = false;
        patrolling();
    }

    void attacking() //transition from patrol to attack, then attack
    {
        Debug.Log("going in for the attack");

        currentlyAttacking = true;
        
        if(theAgent.remainingDistance < 0.2f)
        {
            Debug.Log("attacking!!!!");

            // pHC.ChangeHealth(-15.0f);
            // pHC.TakeDamage();   
        }
        else if(theAgent.remainingDistance < (rangeUsed*1.5))
        {
            wasAttacking();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            pHC.ChangeHealth(-15.0f);
            pHC.TakeDamage();
            Debug.Log("Hit Player");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeUsed);
    }
}
