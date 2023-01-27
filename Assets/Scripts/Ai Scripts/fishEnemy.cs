using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEnemy : MonoBehaviour
{
    public Transform[] patrolPositions;
    public float chaseSpeed = 2.0F;
    public float patrolSpeed = 2.0F;
    [SerializeField]private GameObject playerDiver;
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject mainCam;
    [SerializeField]private GameObject jumpscareCam;
    [SerializeField]private GameObject deathObject;
    [SerializeField]private Animator animator;
    PlayerHealthController pHC;

    [SerializeField] private GameObject gen1;
    [SerializeField] private GameObject gen2;
    [SerializeField] private GameObject gen3;
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
    public bool backToStart = false;
    private bool movingToNextPosition = false;

    Vector3 destination;
    private State state;
    private enemyFieldOfView eFOV;

    private enum State
    {
        attacking, patrolling
    }
    
    void Awake()
    {
        this.GetComponent<AudioSource>();
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
                phase1();
                break;
            case State.attacking:
                Attacking();
                phase1();
                break;
        }
        
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
        this.transform.position = Vector3.MoveTowards(transform.position, destination, patrolSpeed*Time.deltaTime);
        totalLength = Vector3.Distance(this.transform.position, destination);
        
        if (totalLength <= 0.01f)
        {
            movingToNextPosition = false;
        }

        if (eFOV.canSeePlayer)
        {
            state = State.attacking;
        }
    }

    private void Attacking()
    {
        RotateTowards(playerDiver.transform.position);
        this.transform.position = Vector3.MoveTowards(this.transform.position, playerDiver.transform.position, Time.deltaTime*chaseSpeed);
        if (!eFOV.canSeePlayer)
        {
            state = State.patrolling;
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime *5f);
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
            deathObject.SetActive(true);
            cc.enabled = false;
            eelDead = true;
            Debug.Log("eel dead");
            animator.SetBool("isDead", true);
            chaseSpeed = 0;
            patrolSpeed = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            chaseSpeed = 0;
            patrolSpeed = 0;
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("Jumpscare");
        }
    }
}