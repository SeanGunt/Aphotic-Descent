using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ffScr : MonoBehaviour
{
    public NavMeshAgent theAgent;
    [SerializeField] public float agentSpeed;
    [SerializeField] private float scentRange;
    [SerializeField] private float rangeForBleedMultiplier;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] points;
    [SerializeField] public float stunTime;
    private float bleedRange;
    private float rangeUsed;
    private float baseAttackTime = 2.0f;
    Vector3 destination;
    private float playerDistance;
    private bool unchosen = true;
    PlayerHealthController pHC;
    [HideInInspector] public bool currentlyAttacking = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject mainCam, jumpscareCam;
    [SerializeField] private GameObject playerDiver;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip stingerMusic;
    [SerializeField] private BGMManager bGMManager;

    private State state;
    public enum State
    {
        attacking, patrolling, wasAttacking
    }

    
    void Awake()
    {
        theAgent = GetComponent<NavMeshAgent>();
        theAgent.speed = agentSpeed;
        theAgent.updateRotation = true;
        theAgent.autoBraking = false;
        theAgent.acceleration = 250;
        theAgent.angularSpeed = 250;

        audioSource = this.GetComponent<AudioSource>();

        state = State.patrolling;

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
        switch (state)
        {
            default:
            case State.patrolling:
                    patrolling();
            break;
            case State.attacking:
                    attacking();
            break;
            case State.wasAttacking:
                    wasAttacking();
            break;
        }

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
            state = State.patrolling;
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

        if(playerDistance < rangeUsed*rangeUsed)
        {
            bGMManager.SwitchBGMFade(1);
            state = State.attacking;
        }
    }

    void wasAttacking() //transition from attack to patrol
    {
        currentlyAttacking = false;
        state = State.patrolling;
    }

    void attacking() //transition from patrol to attack, then attack
    {
        theAgent.destination = player.transform.position;
        currentlyAttacking = true;
        baseAttackTime -= Time.deltaTime;

        if(playerDistance > rangeUsed*rangeUsed && baseAttackTime <= 0f)
        {
            baseAttackTime = 2.0f;
            bGMManager.SwitchBGMFade(0);
            state = State.wasAttacking;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(stingerMusic);
            theAgent.speed = 0;
            FreakFishGrowling.hitPlayer = true;
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("jumpscare");
        }

        if (other.gameObject.tag == "Knife")
        {
            int randomNoise = Random.Range(0,4);
            audioSource.PlayOneShot(hurtSounds[randomNoise]);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeUsed);
    }
}
