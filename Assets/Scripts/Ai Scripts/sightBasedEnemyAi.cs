using UnityEngine;
using UnityEngine.AI;

public class sightBasedEnemyAi : MonoBehaviour
{
    private State state;
    private NavMeshAgent agent;
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject player;
    private InvisibilityMechanic pS;
    private enemyFieldOfView eFOV;
    enum State
    {
        patrolling, attacking, idle
    }

    private void Awake()
    {
        walkPointSet = false;
        state = State.idle;
        agent = GetComponent<NavMeshAgent>();
        pS = player.GetComponent<InvisibilityMechanic>();
        eFOV = GetComponent<enemyFieldOfView>();
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

        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
        }

        if (eFOV.canSeePlayer && !pS.isSafe)
        {
            state = State.attacking;
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
        agent.SetDestination(player.transform.position);

        if (!eFOV.canSeePlayer || pS.isSafe)
        {
            state = State.patrolling;
        }
    }

    private void Idle()
    {
        if (eFOV.canSeePlayer && !pS.isSafe)
        {
            state = State.attacking;
        }
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

            case State.idle:
                Idle();
            break;
        }
    }
}