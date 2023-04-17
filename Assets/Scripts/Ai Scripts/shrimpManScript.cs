using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class shrimpManScript : MonoBehaviour
{
    public NavMeshAgent shrimpAgent;
    private AudioSource audioSource;
    [SerializeField] private AudioClip shrimpManStinger;
    [SerializeField] public float agentSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float rangeForBleedMultiplier;
    [SerializeField] private GameObject player, shrimpMesh, playerDiver;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] public float stunTime;
    [SerializeField] private GameObject mainCam, jumpscareCam;
    private Animator animator;
    private float bleedRange;
    private float rangeUsed;
    Vector3 destination;
    [SerializeField]private Transform underMudPosition;
    private float playerDistance;
    private bool unchosen = true;
    private bool canGoUnderMud, transitioning, patrolling, goingDown, goingUp;
    PlayerHealthController pHC;
    private InvisibilityMechanic invisibilityMechanic;
    [HideInInspector] public bool currentlyAttacking = false;
    private State state;

    public enum State
    {
        attacking, patrolling, wasAttacking, transitioningIn, transitioningOut 
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        shrimpAgent = GetComponent<NavMeshAgent>();
        shrimpAgent.speed = agentSpeed;
        shrimpAgent.updateRotation = true;
        shrimpAgent.autoBraking = false;
        shrimpAgent.acceleration = 250;
        shrimpAgent.angularSpeed = 250;
        canGoUnderMud = true;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("ShrimpyState",0);

        state = State.patrolling;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            pHC = player.GetComponent<PlayerHealthController>();
            invisibilityMechanic = player.GetComponent<InvisibilityMechanic>();
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
            case State.transitioningIn:
                Transitioning();
                break;
            case State.transitioningOut:
                Transitioning();
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
        patrolling = true;
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

        if(playerDistance < rangeUsed*rangeUsed && !invisibilityMechanic.isInvisible)
        {
            shrimpMesh.transform.position = this.transform.position;
            shrimpAgent.speed = agentSpeed;
            animator.SetInteger("ShrimpyState", 0);
            patrolling = false;
            BreathingManager.instance.SwitchBreathRate(2);
            state = State.attacking;
        }
    }

    void Attacking()
    {
        shrimpAgent.destination = player.transform.position;
        currentlyAttacking = true;
        canGoUnderMud = false;

        if(invisibilityMechanic.isInvisible)
        {
            state = State.patrolling;
            BreathingManager.instance.SwitchBreathRate(0);
        }

        if(playerDistance > rangeUsed*rangeUsed)
        {
            state = State.wasAttacking;
            BreathingManager.instance.SwitchBreathRate(0);
        }
    }

    void WasAttacking()
    {
        currentlyAttacking = false;
        state = State.patrolling;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !invisibilityMechanic.isSafe)
        {
            audioSource.PlayOneShot(shrimpManStinger);
            shrimpAgent.speed = 0;
            BreathingManager.instance.StopBreathe();
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("Jumpscare");
        }

        if (other.gameObject.tag == "Mud" && canGoUnderMud)
        {
            shrimpAgent.speed = (agentSpeed*1.5f);
            transitioning = true;
            state = State.transitioningIn;
            goingDown = true;
            CancelInvoke("TransitionAnim");
            Invoke("TransitionAnim", .001f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mud")
        {
            shrimpAgent.speed = agentSpeed;
            transitioning = true;
            state = State.transitioningOut;
            goingUp = true;
            CancelInvoke("TransitionAnim");
            Invoke("TransitionAnim", .001f);
        }
    }

    private void Transitioning()
    {
        if (patrolling)
        {
            Patrolling();
        }
        if (currentlyAttacking)
        {
            Attacking();
        }
        float timeElapsed = 0;
        if (transitioning)
        {
            if (timeElapsed < 2f)
            {
                float t = timeElapsed/2;
                timeElapsed += Time.deltaTime;
            }
            
            if (timeElapsed == 2f)
            {
                transitioning = false;
            }
        }
        if(!transitioning && patrolling)
        {
            state = State.patrolling;
            animator.SetInteger("ShrimpyState", 0);
        }
        if(!transitioning && currentlyAttacking)
        {
            state = State.attacking;
            animator.SetInteger("ShrimpyState", 0);
        }
        if(playerDistance < rangeUsed*rangeUsed)
        {
            shrimpMesh.transform.position = this.transform.position;
            shrimpAgent.speed = agentSpeed;
            state = State.attacking;
            animator.SetInteger("ShrimpyState", 0);
            patrolling = false;
        }
    }

    private void TransitionAnim()
    {
        if (goingDown)
        {
            animator.SetInteger("ShrimpyState",1);
            goingDown = false;
        }

        if (goingUp)
        {
            animator.SetInteger("ShrimpyState",2);
            goingUp = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeUsed);
    }
}
