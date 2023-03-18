using System.Collections;
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
    [SerializeField]private Transform underMudPosition;
    private float playerDistance;
    private bool unchosen = true;
    private bool canGoUnderMud, transitioning;
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
        canGoUnderMud = true;

        state = State.patrolling;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            pHC = player.GetComponent<PlayerHealthController>();
        }

        bleedRange = detectionRange * rangeForBleedMultiplier;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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

        canGoUnderMud = true;

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
        canGoUnderMud = false;

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

        if (other.gameObject.tag == "Mud" && canGoUnderMud)
        {
            shrimpAgent.speed = (agentSpeed*2.0f);
            StopAllCoroutines();
            StartCoroutine(TransitionDaShrimp(this.transform.position, underMudPosition.transform.position));
            transitioning = true;
            Debug.Log("Mud entered.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mud")
        {
            shrimpAgent.speed = agentSpeed;
            StopAllCoroutines();
            StartCoroutine(TransitionDaShrimp(underMudPosition.transform.position, this.transform.position));
            Debug.Log("Mud exited.");
        }
    }

    private IEnumerator TransitionDaShrimp(Vector3 start, Vector3 end)
    {
        float timeElapsed = 0;
        
        while (timeElapsed < 2)
        {
            float t = timeElapsed/2;
            shrimpMesh.transform.position = Vector3.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = end;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeUsed);
    }
}
