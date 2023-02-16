using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    private float idleTimer, distanceToFear, randomSize, timeTillVelocityIsCalculated = 2f, velocity;
    [SerializeField] private float enemyDistanceRun, minIdleTime, maxIdleTime, walkPointRange, patrolSpeed, fearedSpeed;
    [SerializeField] private LayerMask whatIsGround;
    private bool idleTimerReset, walkPointSet, velocityIsBeingCalculated;
    private Vector3 walkPoint, previousPosition;
    private State state;
    private enum State
    {
        patrol, idle, feared
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
        randomSize = Random.Range(1f,2f);
        this.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        state = State.patrol;
    }
    void Update()
    {

        switch(state)
        {
            case State.patrol:
                Patrolling();
            break;
            case State.idle:
                Idle();
            break;
            case State.feared:
                Feared();
            break;
        }
    }

    private void Idle()
    {
        DetectPlayer();
        timeTillVelocityIsCalculated = 2f;
        velocityIsBeingCalculated = false;
        agent.speed = 0f;
        walkPointSet = false;
        if (!idleTimerReset)
        {
            idleTimer = Random.Range(minIdleTime, maxIdleTime);
            idleTimerReset = true;
        }

        idleTimer -= Time.deltaTime;
        if(idleTimer <= 0)
        {   
            state = State.patrol;
        }
        
    }
    private void Patrolling()
    {
        DetectPlayer();
        agent.speed = patrolSpeed;
        idleTimerReset = false;
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            walkPointSet = false;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        timeTillVelocityIsCalculated -= Time.deltaTime;

        if(timeTillVelocityIsCalculated <= 0)
        {
            velocityIsBeingCalculated = true;
        }

        if (velocityIsBeingCalculated)
        {
            velocity = Vector3.Distance(this.transform.position, previousPosition);
            previousPosition = this.transform.position;
        }
        else
        {
            velocity = 1f;
        }

        if (distanceToWalkPoint.magnitude < 1.0f || velocity == 0)
        {
            state = State.idle;
        }
    }

    private void Feared()
    {
        agent.speed = fearedSpeed;
        Vector3 dirToPlayer = transform.position - player.transform.position;
        Vector3 newPos = transform.position + dirToPlayer;
        agent.SetDestination(newPos);

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            state = State.patrol;
        }

        float distanceToDestination = Vector3.Distance(transform.position, agent.destination);
        if (distanceToDestination <= 0.5f)
        {
            state = State.idle;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void DetectPlayer()
    {
        distanceToFear = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToFear < enemyDistanceRun)
        {
            state = State.feared;
        }
    }
}