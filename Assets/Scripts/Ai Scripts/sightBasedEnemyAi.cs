using UnityEngine;
using UnityEngine.AI;

public class sightBasedEnemyAi : MonoBehaviour
{
    private State state;
    private NavMeshAgent agent;
    private bool walkPointSet;
    private Vector3 walkPoint;
    [SerializeField] private Transform[] walkPoints;
    private int randomSpot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject player;
    private InvisibilityMechanic pS;
    private enemyFieldOfView eFOV;
    [SerializeField] private BGMManager bGMManager;
    enum State
    {
        patrolling, attacking, idle
    }

    private void Awake()
    {
        walkPointSet = false;
        state = State.patrolling;
        agent = GetComponent<NavMeshAgent>();
        pS = player.GetComponent<InvisibilityMechanic>();
        eFOV = this.GetComponent<enemyFieldOfView>();
        if(eFOV != null)
        {
            Debug.Log("eFoV enabled");
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Debug.Log(agent.remainingDistance);

        Vector3 distanceToWalkPoint =  transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 10.0f)
        {
            walkPointSet = false;
        }

        if (eFOV.canSeePlayer && !pS.isSafe)
        {
            state = State.attacking;
            bGMManager.state = BGMManager.State.EelChase;
        }
    }

    private void SearchWalkPoint()
    {
        if (!walkPointSet)
        {
            randomSpot = Random.Range(0, walkPoints.Length);
            walkPoint = walkPoints[randomSpot].position;
            walkPointSet = true;
        }
    }

    private void Attacking()
    {
        agent.SetDestination(player.transform.position);

        if (!eFOV.canSeePlayer || pS.isSafe)
        {
            bGMManager.state = BGMManager.State.EelIdle;
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
                SearchWalkPoint();
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