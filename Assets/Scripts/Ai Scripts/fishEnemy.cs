using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEnemy : MonoBehaviour
{
    public Transform[] patrolPositions;
    public float chaseSpeed = 2.0F;
    public float patrolSpeed = 2.0F;
    public float chaseSpeed2 = 12.5f;
    public float patrolSpeed2 = .8f;

    [SerializeField]private GameObject playerDiver, player, playerHead, mainCam, jumpscareCam, deathObject, deathCube;
    [SerializeField]private Animator animator;
    PlayerHealthController pHC;
    InvisibilityMechanic iM;
    RaycastHit hit;
    [SerializeField]private LayerMask doNotIgnoreLayer, playerLayer;
    [SerializeField] private GameObject gen1, gen2, gen3, gen4;
    private CapsuleCollider cc;
    generatorScript gen1Scr;
    generatorScript gen2Scr;
    generatorScript gen3Scr;
    generatorScript gen4Scr;
    generatorScript dethCubeScr;
    private bool g1On = true;
    private bool g2On = true;
    private bool g3On = true;
    private bool g4On = true;
    private int eelHealth = 4;
    [SerializeField] private int phase;
    private bool eelDead = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] eelSounds;
    private float beginningTime;
    private float totalLength;
    private float randomTime;
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
        attacking, patrolling, repositioning, dead, killedPlayer, lockingIn, stuck, idle, transitioning
    }
    
    void Awake()
    {
        randomTime = Random.Range(4f,9f);
        audioSource = this.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        cc = GetComponent<CapsuleCollider>();
        beginningTime = Time.time;
        eFOV = this.GetComponent<enemyFieldOfView>();
        animator = GetComponentInChildren<Animator>();
        phase = 1;
        if(player != null)
        {
            pHC = player.GetComponent<PlayerHealthController>();
            iM = player.GetComponent<InvisibilityMechanic>();

            gen1Scr = gen1.GetComponent<generatorScript>();
            gen2Scr = gen2.GetComponent<generatorScript>();
            gen3Scr = gen3.GetComponent<generatorScript>();
            gen4Scr = gen4.GetComponent<generatorScript>();
        }

        state = State.patrolling;
    }

    void OnEnable()
    {
        state = State.patrolling;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnDisable()
    {
        state = State.idle;
    }

    void Update()
    {
        switch(state)
        {
            default:
            case State.patrolling:
                Patrolling();
                RepositionCheck();
                if (phase == 1)
                {
                    Phase1();
                }
                else
                {
                    Phase2();
                }
                break;
            case State.attacking:
                Attacking();
                if (phase == 1)
                {
                    Phase1();
                }
                else
                {
                    Phase2();
                }
                break;
            case State.repositioning:
                Repositioning();
                RepositionCheck();
                if (phase == 1)
                {
                    Phase1();
                }
                else
                {
                    Phase2();
                }
                break;
            case State.killedPlayer:
                break;
            case State.transitioning:
                Invoke("TransitionPhase2", 2);
                break;
            case State.idle:
                Idle();
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

        randomTime -=  Time.deltaTime;
        if (randomTime <= 0)
        {
            audioSource.PlayOneShot(eelSounds[2]);
            randomTime = Random.Range(4f,9f);
        }
        
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
            audioSource.PlayOneShot(eelSounds[3]);
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

    void Phase1()
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

        if(gen4Scr.isOn == false && g4On)
        {
            Debug.Log("gen 4 off");
            eelHealth = eelHealth -1;
            g4On = false;
        }
        
        if((eelHealth == 0) && (!g1On && !g2On && !g3On && !g4On) && (eelDead == false))
        {
            BGMManager.instance.SwitchBGM(0);
            animator.SetBool("isDying", true);
            eFOV.enabled = false;
            state = State.transitioning;
        }
    }

    void Phase2()
    {
        deathCube.SetActive(true);
    }

    void TransitionPhase2()
    {   
        animator.SetBool("isReviving", true);
        animator.SetBool("isDying", false);
        BGMManager.instance.SwitchBGM(2);
        Invoke("Transitioning", 2);
    }

    void Transitioning()
    {
        animator.SetBool("isBack", true);
        animator.SetBool("isReviving", false);
        Invoke("Transitioned", 2);
    }

    void Transitioned()
    {
        animator.SetBool("isBack", false);
        phase = 2;
        chaseSpeed = chaseSpeed2;
        patrolSpeed = patrolSpeed2;
        state = State.patrolling;
    }

    // void Transition()
    // {
    //     phase = 2;
    //     chaseSpeed = chaseSpeed2;
    //     patrolSpeed = patrolSpeed2;
    //     state = State.patrolling;
    // }

    void Idle()
    {
        movingToNextPosition = false;
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
            audioSource.PlayOneShot(eelSounds[0]);
            chaseSpeed = 0;
            patrolSpeed = 0;
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("Jumpscare");
        }
    }
}