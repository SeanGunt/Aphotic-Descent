using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEnemy : MonoBehaviour
{
    public Transform[] patrolPositions;
    public float chaseSpeed = 2.0F;
    public float patrolSpeed = 2.0F;
    [SerializeField]private GameObject playerDiver, player, playerHead, mainCam, jumpscareCam, deathObject;
    [SerializeField]private Animator animator;
    PlayerHealthController pHC;
    InvisibilityMechanic iM;
    RaycastHit hit;
    [SerializeField]private LayerMask doNotIgnoreLayer, playerLayer;
    [SerializeField] private GameObject gen1, gen2, gen3;
    private CapsuleCollider cc;
    generatorScript gen1Scr;
    generatorScript gen2Scr;
    generatorScript gen3Scr;
    private bool g1On = true;
    private bool g2On = true;
    private bool g3On = true;
    private int eelHealth = 3;
    private bool eelDead = false;
    private AudioSource audioSource;
    private float beginningTime;
    private float totalLength;
    [SerializeField] private float amount;
    public bool backToStart = false;
    private bool movingToNextPosition = false;
    [SerializeField]private bool unobstructed, repositioning;
    Vector3 destination;
    private State state;
    private enemyFieldOfView eFOV;
    Vector3 lastPosition;

    private enum State
    {
        attacking, patrolling, repositioning, dead, killedPlayer
    }
    
    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        cc = GetComponent<CapsuleCollider>();
        beginningTime = Time.time;
        eFOV = this.GetComponent<enemyFieldOfView>();
        animator = GetComponentInChildren<Animator>();
        if (eFOV != null)
        {
            Debug.Log("eFoV enabled");
        }
        if(player != null)
        {
            Debug.Log("player found");
            pHC = player.GetComponent<PlayerHealthController>();
            iM = player.GetComponent<InvisibilityMechanic>();

            gen1Scr = gen1.GetComponent<generatorScript>();
            gen2Scr = gen2.GetComponent<generatorScript>();
            gen3Scr = gen3.GetComponent<generatorScript>();
        }

        state = State.patrolling;

    }

    void Update()
    {
        switch(state)
        {
            default:
            case State.patrolling:
                Patrolling();
                RepositionCheck();
                phase1();
                break;
            case State.attacking:
                Attacking();
                phase1();
                break;
            case State.repositioning:
                Repositioning();
                RepositionCheck();
                phase1();
                break;
            case State.killedPlayer:
                break;
            case State.dead:
                Dead();
                break;
        }
        HandleTailWiggleSpeed();
    }

    private void Patrolling()
    {
        if (patrolPositions.Length == 0)
        {
            return;
        }

        if(!movingToNextPosition)
        {
            destination = patrolPositions[Random.Range(0,patrolPositions.Length)].position;
            movingToNextPosition = true;
        }
        RotateTowards(destination);
        this.transform.position = Vector3.Slerp(transform.position, destination, patrolSpeed * Time.deltaTime);
        totalLength = Vector3.Distance(this.transform.position, destination);
        
        if (totalLength <= 10f)
        {
            movingToNextPosition = false;
            repositioning = false;
            unobstructed = true;
        }

        if (eFOV.canSeePlayer && !repositioning && !iM.isSafe)
        {
            BGMManager.instance.SwitchBGM(4);
            BreathingManager.instance.SwitchBreathRate(2);
            state = State.attacking;
        }
    }

    private void Attacking()
    {
        RotateTowards(playerHead.transform.position);
        this.transform.position = Vector3.MoveTowards(this.transform.position, playerHead.transform.position, Time.deltaTime * chaseSpeed);
        if (!eFOV.canSeePlayer || iM.isSafe)
        {
            BreathingManager.instance.SwitchBreathRate(0);
            state = State.patrolling;
            BGMManager.instance.SwitchBGM(3);
        }
    }

    private void Dead()
    {
        deathObject.SetActive(true);
        cc.enabled = false;
        eFOV.enabled = false;
        eelDead = true;
        animator.SetBool("isDead", true);
        chaseSpeed = 0;
        patrolSpeed = 0;
    }

    private void Repositioning()
    {
        if (patrolPositions.Length == 0)
        {
            return;
        }

        if(!movingToNextPosition)
        {
            destination = patrolPositions[Random.Range(0,patrolPositions.Length)].position;
            movingToNextPosition = true;
        }
        if(repositioning)
        {
            destination = patrolPositions[Random.Range(0,patrolPositions.Length)].position;
            movingToNextPosition = true;
            repositioning = false;
        }
        RotateTowards(destination);
        this.transform.position = Vector3.MoveTowards(transform.position, destination, chaseSpeed * Time.deltaTime);
        totalLength = Vector3.Distance(this.transform.position, destination);
        
        if (totalLength <= 10f)
        {
            movingToNextPosition = false;
            unobstructed = true;
            state = State.patrolling;
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * 2f);
    }

    void phase1()
    {
        if(gen1Scr.isOn == false && g1On)
        {
            Debug.Log("gen 1 off");
            eelHealth = eelHealth -1;
            g1On = false;
        }

        if(gen2Scr.isOn == false && g2On)
        {
            Debug.Log("gen 2 off");
            eelHealth = eelHealth -1;
            g2On = false;
        }

        if(gen3Scr.isOn == false && g3On)
        {
            Debug.Log("gen 3 off");
            eelHealth = eelHealth -1;
            g3On = false;
        }
        
        if((eelHealth == 0) && (!g1On && !g2On && !g3On) && (eelDead == false))
        {
            BGMManager.instance.SwitchBGM(0);
            state = State.dead;
        }
    }

    void RepositionCheck()
    {
        Vector3 centerRay = transform.TransformDirection(new Vector3( 0, 0, 1))* amount/4;
        Vector3 rightRay = transform.TransformDirection(new Vector3( 1, 0, 1))* amount/5;
        Vector3 leftRay = transform.TransformDirection(new Vector3(-1, 0, 1))* amount/5;

        Debug.DrawRay(transform.position, rightRay, Color.red);
        Debug.DrawRay(transform.position, centerRay, Color.red);
        Debug.DrawRay(transform.position, leftRay, Color.red);

        if((Physics.Raycast(transform.position,rightRay,out hit, amount, doNotIgnoreLayer) || 
            Physics.Raycast(transform.position,leftRay, out hit, amount, doNotIgnoreLayer) ||
            Physics.Raycast(transform.position,centerRay, out hit, amount/2, doNotIgnoreLayer)) == false)
            {
                unobstructed = true;               
            }
        else
        {
            if (unobstructed)
            {
                repositioning = true;
                state = State.repositioning;
                unobstructed = false;
            }
        }
    }

    private void HandleTailWiggleSpeed()
    {
        float velocity = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        animator.SetFloat("speed", velocity * 8);
    }
    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            state = State.killedPlayer;
            BreathingManager.instance.StopBreathe();
            chaseSpeed = 0;
            patrolSpeed = 0;
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("Jumpscare");
        }
    }
}