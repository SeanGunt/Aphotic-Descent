using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemyAi : MonoBehaviour
{
    private State state;
    private NavMeshAgent agent;
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    [SerializeField] private LayerMask whatIsGround;
    enum State
    {
        patrolling, attacking
    }

    private void Awake()
    {
        walkPointSet = false;
        state = State.patrolling;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.destination = walkPoint;
        }

        Vector3 distanceToWalkPoint =  transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 2.5f)
        {
            walkPointSet = false;
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

    private void Attacking()
    {

    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.patrolling:
                Patrolling();
            break;

            case State.attacking:
                Attacking();
            break;
        }
    }
}
