using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class anglerAi : MonoBehaviour
{
    //
    // HEY the trigger this is connected to is located on the diver lure portion of the angler
    // AnglerTotal->Armature->Bone.010->...Bone.020

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float anglerRange;
    [SerializeField] private float anglerRangeMultiplier; //how much will angler's vision be multiplied by?
    [SerializeField] private float kbBlacklightForce;
    [SerializeField] private float kbKnifeForceMultiplier; //how hard will angler be knocked back by knife?
    [SerializeField] private float attackCountDown;
    [SerializeField] private float aoeDamageAmount; //how much will the player's health be subtracted by?
    [SerializeField] private GameObject lightObjectPoint;
    public float anglerAttackRange;
    private float resetCountDown;
    public NavMeshAgent anglerAgent;
    private float resetForce;
    private float resetAnglerRange;
    private GameObject player;
    private int currentPoint = 0;
    private bool unchosen;
    public bool isAlive = true;
    public bool isInvestigating = false;
    public float anglerStunTime;
    private float distBtwn;
    public float investTimer;
    private float resetInvestTimer;
    private bool aoeAttackActivated = false;
    private Animation animaTor;
    [HideInInspector] public float anglerSpeed;
    [HideInInspector] public PlayerHealthController pHelCon;
    [HideInInspector] public bool isStunned = false;
    blacklightKnockback blKb;
    enemyFieldOfView eFovScr1, eFovScr2; //the other is on the diver trigger


    public State state;
    public enum State
    {
        anglerPatrolling, anglerDead, anglerAttacking, anglerInvestigate
    }

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

        animaTor = this.GetComponent<Animation>();
        
        anglerSpeed = anglerAgent.speed;
        resetAnglerRange = anglerRange;

        //lightObjectPoint = GameObject.Find("anglerLightObjectPoint");
        resetCountDown = attackCountDown;
        lightObjectPoint.SetActive(false);

        blKb.knockbackForce = kbBlacklightForce;
        resetForce = kbBlacklightForce;
        eFovScr1.playerRef = player;
        eFovScr2.playerRef = player;
        eFovScr1.radius = anglerRange;
    }

    void Update()
    {
        distBtwn = Vector3.Distance(player.transform.position, lightObjectPoint.transform.position);
        
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

        if(isInvestigating)
        {
            state = State.anglerInvestigate;
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
            case State.anglerInvestigate:
                investigate();
            break;
        }
    }

    void patrolling()
    {
        if(isAlive)
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

            if(eFovScr1.canSeePlayer || eFovScr2.canSeePlayer)
            {
                unchosen = false;
                state = State.anglerAttacking;
            }
        }
        else
        {
            state = State.anglerDead;
        }
    }

    private void Dead()
    {
        unchosen = false;
        anglerAgent.ResetPath();
        anglerAgent.speed = 0;
        anglerAgent.acceleration = 0;
        isAlive = false;
    }

    private void attacking()
    {
        if(isAlive)
        {
            if(eFovScr1.canSeePlayer || eFovScr2.canSeePlayer)
            {
                unchosen = false;
                anglerAgent.destination = player.transform.position;

                if(distBtwn <= anglerAttackRange)
                {
                    aoeAttackActivated = true;
                    attackPlayer();
                }
            }
            else if(eFovScr1.canSeePlayer == false || eFovScr2.canSeePlayer == false)
            {
                unchosen = true;
                aoeAttackActivated = false;
                anglerAgent.ResetPath();
                state = State.anglerPatrolling;
                lightObjectPoint.SetActive(false);
                
            }
        }
        else
        {
            state = State.anglerDead;
        }
    }

    void investigate()
    {
        unchosen = false;
        
        if(!anglerAgent.pathPending && anglerAgent.remainingDistance < 0.5f)
        {
            investTimer -= Time.deltaTime;
            if(eFovScr1.canSeePlayer || eFovScr2.canSeePlayer)
            {
                isInvestigating = false;
                state = State.anglerAttacking;
                investTimer = resetInvestTimer;
            }
            else if(investTimer <= 0)
            {
                investTimer = resetInvestTimer;
                isInvestigating = false;
                state = State.anglerPatrolling;
            }
        }
    }

    void attackPlayer()
    {
        if(aoeAttackActivated && (isStunned == false))
        {
            lightObjectPoint.SetActive(true);
            attackCountDown -= Time.deltaTime;
            if(attackCountDown <= 0)
            {
                pHelCon.ChangeHealth((aoeDamageAmount)*-1.0f);
                pHelCon.TakeDamage();
                attackCountDown = resetCountDown;
            }
        }
        else
        {
            attackCountDown = resetCountDown;
            lightObjectPoint.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lightObjectPoint.transform.position, anglerAttackRange);
    }
}