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
    [SerializeField] private float anglerRangeMultiplier; //how much will angler's vision be multiplied by?
    [SerializeField] private float kbBlacklightForce;
    [SerializeField] private float kbKnifeForceMultiplier; //how hard will angler be knocked back by knife?
    private float resetForce;
    private float resetAnglerRange;
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
    enemyFieldOfView eFovScr1, eFovScr2; //the other is on the diver trigger

    public State state;
    public enum State
    {
        anglerPatrolling, anglerDead, anglerAttacking
    }

    // Awake is called when the object the script is attached to becomes enabled
    void Awake()
    {
        state = State.anglerPatrolling;
        anglerAgent = this.GetComponent<NavMeshAgent>();

        anglerAgent.destination = patrolPoints[currentPoint].position;

        player = GameObject.FindGameObjectWithTag("Player");
        pHelCon = player.GetComponent<PlayerHealthController>();
        blKb = GetComponentInChildren<blacklightKnockback>();
        eFovScr1 = GetComponent<enemyFieldOfView>();
        eFovScr2 = GameObject.Find("angLureTrigger").GetComponent<enemyFieldOfView>();
        
        anglerSpeed = anglerAgent.speed;
        resetAnglerRange = anglerRange;

        blKb.knockbackForce = kbBlacklightForce;
        resetForce = kbBlacklightForce;
        eFovScr1.playerRef = player;
        eFovScr2.playerRef = player;
        eFovScr1.radius = anglerRange;

        Debug.Log(eFovScr2.gameObject.name + " is where efov2 is located");
    }

    // Update is called once per frame
    void Update()
    {
        distBtwn = Vector3.Distance(player.transform.position, transform.position);
        
        if(isAlive == false)
        {
            state = State.anglerDead;
        }

        if(player.transform.position.y > this.gameObject.transform.position.y)
        {
            eFovScr1.radius = resetAnglerRange * anglerRangeMultiplier;
        }
        else
        {
            eFovScr1.radius = resetAnglerRange;
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
            if(eFovScr1.canSeePlayer || eFovScr2.canSeePlayer)
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
                //Debug.Log("from diver trigger: knife hit");
                blKb.knockbackForce = blKb.knockbackForce * kbKnifeForceMultiplier;
                blKb.knockingBack();
                blKb.knockbackForce = resetForce;
            }
            
            if(other.gameObject.tag == "Player")
            {
                Debug.Log("from diver trigger: ayyoo wassup");
            }
        }
    }
}