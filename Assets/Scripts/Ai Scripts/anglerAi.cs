using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class anglerAi : MonoBehaviour
{
    //
    // HEY the trigger this is connected to is located on the diver lure portion of the angler
    //

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float anglerRange;
    private GameObject player;
    private int currentPoint = 0;
    public NavMeshAgent anglerAgent;
    private bool unchosen;
    public bool isAlive = true;
    public float anglerStunTime;
    private float distBtwn;
    [HideInInspector] public float anglerSpeed;
    [HideInInspector] public PlayerHealthController pHelCon;
    blacklightKnockback blKb;

    public State state;
    public enum State
    {
        anglerPatrolling, anglerDead, anglerAttacking
    }

    // Start is called before the first frame update
    void Awake()
    {
        state = State.anglerPatrolling;
        anglerAgent = this.GetComponent<NavMeshAgent>();

        anglerAgent.destination = patrolPoints[currentPoint].position;

        player = GameObject.FindGameObjectWithTag("Player");
        pHelCon = player.GetComponent<PlayerHealthController>();
        
        anglerSpeed = anglerAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        distBtwn = Vector3.Distance(player.transform.position, transform.position);
        
        if(isAlive == false)
        {
            state = State.anglerDead;
        }

        switch (state)
        {
            default:
            case State.anglerPatrolling:
                patrolling();
            break;
            case State.anglerAttacking:
                attacking();
            break;
            case State.anglerDead:
                Dead();
            break;
        }
    }

    void patrolling()
    {
        if(!anglerAgent.pathPending && anglerAgent.remainingDistance < 0.5f)
        {            
            unchosen = true;
        }
        
        if(unchosen)
        {
            anglerAgent.destination = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
            unchosen = false;
        }

        if(distBtwn <= anglerRange)
        {
            unchosen = false;
            anglerAgent.ResetPath();
            state = State.anglerAttacking;
        }
    }

    private void Dead()
    {
        //Debug.Log("angler dead");
        unchosen = false;
        anglerAgent.ResetPath();
        anglerAgent.speed = 0;
        anglerAgent.acceleration = 0;
    }

    private void attacking()
    {
        if(isAlive)
        {
            if(distBtwn <= anglerRange)
            {
                unchosen = false;
                anglerAgent.destination = player.transform.position;
            }
            else
            {
                unchosen = true;
                anglerAgent.ResetPath();
                state = State.anglerPatrolling;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(isAlive)
        {    
            if(other.gameObject.tag == "Knife")
            {
                Debug.Log("from diver trigger: knife hit");
                blKb.knockbackForce = blKb.knockbackForce * 3;
                blKb.knockingBack();
            }
            
            if(other.gameObject.tag == "Player")
            {
                Debug.Log("from diver trigger: ayyoo wassup");
            }
        }
    }
}