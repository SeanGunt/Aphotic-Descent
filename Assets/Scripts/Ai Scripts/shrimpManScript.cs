using UnityEngine;
using UnityEngine.AI;

public class shrimpManScript : MonoBehaviour
{
    public NavMeshAgent shrimpAgent;
    [SerializeField] public float agentSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float rangeForBleedMultiplier;
    [SerializeField] private GameObject player, shrimpMesh, playerDiver;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] public float stunTime;

    private float bleedRange;
    private float rangeUsed;
    private float baseAttackTime = 2.0f;
    Vector3 destination;
    private float playerDistance;
    private bool unchosen = true;
    PlayerHealthController pHC;
    [HideInInspector] public bool currentlyAttacking = false;
    
    private State state;

    public enum State
    {
        attacking, patrolling, wasAttacking
    }

    void Awake()
    {
        shrimpAgent = GetComponent<NavMeshAgent>();
        shrimpAgent.speed = agentSpeed;
        shrimpAgent.updateRotation = true;
        shrimpAgent.autoBraking = false;
        shrimpAgent.acceleration = 250;
        shrimpAgent.angularSpeed = 250;

        state = State.patrolling;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            pHC = player.GetComponent<PlayerHealthController>();
        }

        bleedRange = detectionRange * rangeForBleedMultiplier;
    }

    void Update()
    {
        playerDistance = (player.transform.position - this.transform.position).sqrMagnitude;
        switch (state)
        {
            default:
            case State.patrolling:
                Patrolling();
                break;
            case State.attacking:
                Attacking();
                break;
            case State.wasAttacking:
                WasAttacking();
                break;
        }
        if(pHC.isBleeding)
        {
            rangeUsed = bleedRange;
        }
        else
        {
            rangeUsed = detectionRange;
        }

        if((!shrimpAgent.pathPending && shrimpAgent.remainingDistance < 0.5f) && !currentlyAttacking)
        {
            unchosen = true;
            state = State.patrolling;
        }
    }

    void Patrolling()
    {
        if(patrolPoints.Length == 0)
        {
            return;
        }

        if(unchosen == true)
        {
            destination = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
            unchosen = false;
        }

        if (shrimpAgent.pathStatus == NavMeshPathStatus.PathInvalid || shrimpAgent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            unchosen = false;
        }

        shrimpAgent.destination = destination;

        if(playerDistance < rangeUsed*rangeUsed)
        {
            shrimpMesh.transform.position = this.transform.position;
            shrimpAgent.speed = agentSpeed;
            state = State.attacking;
        }
    }

    void Attacking()
    {
        shrimpAgent.destination = player.transform.position;
        currentlyAttacking = true;
        baseAttackTime -= Time.deltaTime;

        if(playerDistance > rangeUsed*rangeUsed && baseAttackTime <= 0f)
        {
            baseAttackTime = 2.0f;
            state = State.wasAttacking;
        }
    }

    void WasAttacking()
    {
        currentlyAttacking = false;
        state = State.patrolling;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            shrimpAgent.speed = 0;
            BreathingManager.instance.StopBreathe();
            pHC.ChangeHealth(-pHC.maxHealth);
            playerDiver.SetActive(false);
        }

        if (other.gameObject.tag == "Mud")
        {
            shrimpAgent.speed = (agentSpeed*2.0f);
            shrimpMesh.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 10, this.transform.position.z);
            Debug.Log("Entered Mud");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mud")
        {
            shrimpAgent.speed = agentSpeed;
            shrimpMesh.transform.position = this.transform.position;
            Debug.Log("Exited Mud");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeUsed);
    }
}
