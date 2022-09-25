using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAi : MonoBehaviour
{
    private State state;
    private NavMeshAgent agent;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private float aggroDistance = 12f;
    [SerializeField] private float walkPointRange;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject player;
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

        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
        }

        float distanceBetween = Vector3.Distance(player.transform.position, this.transform.position);
        if (distanceBetween < aggroDistance)
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

        float distanceBetween = Vector3.Distance(player.transform.position, this.transform.position);
        if (distanceBetween > aggroDistance)
        {
            state = State.patrolling;
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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroDistance);
    }
}
