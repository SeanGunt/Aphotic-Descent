using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEnemy : MonoBehaviour
{
    public Transform[] patrolPositions;
    private int positionInPoints;
    public float chaseSpeed = 2.0F;
    public float patrolSpeed = 2.0F;
    public float chaseSpeed2 = 12.5f;
    public float patrolSpeed2 = .8f;
    private bool canBeBlacklighted;
    [SerializeField]private GameObject playerDiver, player, playerHead, mainCam, jumpscareCam, deathObject, deathCube, barnacleHolder, deadEel;
    [SerializeField]private GameObject[] barnacles;
    [SerializeField]private float currentScale, maxScale, trackingCooldown, stunTime;
    private float maxStunTime, maxTrackingCooldown;
    [SerializeField]private Animator animator;
    PlayerHealthController pHC;
    InvisibilityMechanic iM;
    [SerializeField] private GameObject gen1, gen2, gen3, gen4, bolt, boltSpark;
    private CapsuleCollider cc;
    generatorScript gen1Scr;
    generatorScript gen2Scr;
    generatorScript gen3Scr;
    generatorScript gen4Scr;
    boltScript boltScr;
    private bool g1On = true;
    private bool g2On = true;
    private bool g3On = true;
    private bool g4On = true;
    private bool boltOn;
    private int eelHealth = 4;
    [SerializeField]private int barnacleCount;
    [SerializeField] private int phase;
    private bool eelDead = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] eelSounds;
    [SerializeField] private AudioClip eelStinger;
    private float beginningTime;
    private float totalLength;
    private float randomTime;
    public bool backToStart = false;
    private bool movingToNextPosition = false;
    [SerializeField]private bool isGrowing, playerHid;
    Vector3 destination;
    private State state;
    private enemyFieldOfView eFOV;
    [SerializeField]private ObjectiveUpdateHolder objectiveUpdater;
    Vector3 lastPosition;

    private enum State
    {
        attacking, patrolling, dead, killedPlayer, stunned, idle, transitioning, lockingOn
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
        currentScale = 1;
        maxScale = 6;
        positionInPoints = 0;
        maxTrackingCooldown = trackingCooldown;
        maxStunTime = stunTime;
        barnacleCount = 6;
        isGrowing = false;
        if(player != null)
        {
            pHC = player.GetComponent<PlayerHealthController>();
            iM = player.GetComponent<InvisibilityMechanic>();

            gen1Scr = gen1.GetComponent<generatorScript>();
            gen2Scr = gen2.GetComponent<generatorScript>();
            gen3Scr = gen3.GetComponent<generatorScript>();
            gen4Scr = gen4.GetComponent<generatorScript>();
            boltScr = bolt.GetComponent<boltScript>();
            boltScr.isOn = false;
        }

        if(GameDataHolder.eelIsDead)
        {
            deadEel.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            gen1Scr.enabled = false;
            gen2Scr.enabled = false;
            gen3Scr.enabled = false;
            gen4Scr.enabled = false;
            boltScr.enabled = false;
        }

        state = State.patrolling;
    }

    void OnEnable()
    {
        state = State.patrolling;
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
            case State.killedPlayer:
                break;
            case State.transitioning:
                GrowBarnacles();
                break;
            case State.stunned:
                StunnedEel();
                Phase2();
                break;
            case State.lockingOn:
                LockingOn();
                Phase2();
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
            destination = patrolPositions[positionInPoints].position;
            positionInPoints++;
            if(positionInPoints == patrolPositions.Length)
            {
                positionInPoints = 0;
            }
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
        
        if (totalLength <= 5f)
        {
            movingToNextPosition = false;
        }

        if (eFOV.canSeePlayer && !iM.isSafe && !playerHid && phase == 1)
        {
            BGMManager.instance.SwitchBGM(4);
            BreathingManager.instance.SwitchBreathRate(2);
            audioSource.PlayOneShot(eelSounds[3]);
            state = State.attacking;
        }

        if (eFOV.canSeePlayer && !iM.isSafe && !playerHid && phase == 2)
        {
            //BGMManager.instance.SwitchBGM(4);
            BreathingManager.instance.SwitchBreathRate(2);
            audioSource.PlayOneShot(eelSounds[3]);
            Invoke("StartAttacking", 5);
            state = State.lockingOn;
        }
        
        trackingCooldown = Mathf.Clamp(trackingCooldown, 0, maxTrackingCooldown);
        if (playerHid)
        {
            trackingCooldown -= Time.deltaTime;
            if (trackingCooldown <= 0)
            {
                trackingCooldown = maxTrackingCooldown;
                playerHid = false;
            }
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
            if (phase != 2)
            {
                BGMManager.instance.SwitchBGM(3);
            }
            playerHid = true;
        }
    }

    private void LockingOn()
    {
        if (!eFOV.canSeePlayer || iM.isSafe)
        {
            BreathingManager.instance.SwitchBreathRate(0);
            if (phase != 2)
            {
                BGMManager.instance.SwitchBGM(4);
            }
            playerHid = true;
            CancelInvoke("StartAttacking");
            state = State.patrolling;
        }
        RotateTowards(playerHead.transform.position);
    }

    private void Dead()
    {
        cc.enabled = false;
        eFOV.enabled = false;
        eelDead = true;
        GameDataHolder.eelIsDead = true;
        animator.SetBool("isDead", true);
        chaseSpeed = 0;
        patrolSpeed = 0;
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
            Invoke("TransitionPhase2", 3);
            BGMManager.instance.SwitchBGMFade(12);
            animator.SetBool("isDying", true);
            eFOV.enabled = false;
            state = State.transitioning;
            audioSource.PlayOneShot(eelSounds[4]);
        }
    }

    void TransitionPhase2()
    {   
        animator.SetBool("isReviving", true);
        animator.SetBool("isDying", false);
        Invoke("Transitioning", 3);
        barnacleHolder.SetActive(true);
    }

    void Transitioning()
    {
        animator.SetBool("isBack", true);
        animator.SetBool("isReviving", false);
        Invoke("Transitioned", 3);
        isGrowing = true;
    }

    void Transitioned()
    {
        foreach(GameObject barnacle in barnacles)
        {
            MeshCollider col;
            col = barnacle.GetComponent<MeshCollider>();
            col.enabled = true;
        }
        canBeBlacklighted = true;
        BGMManager.instance.SwitchBGMFade(13);
        animator.SetBool("isBack", false);
        phase = 2;
        objectiveUpdater.SixthObjective();
        eelHealth = 1;
        boltOn = true;
        boltScr.isOn = true;
        boltSpark.SetActive(true);
        chaseSpeed = chaseSpeed2;
        patrolSpeed = patrolSpeed2;
        playerHid = true;
        eFOV.enabled = true;
        state = State.patrolling;
    }

    void Phase2()
    {
        if(state == State.stunned)
        {
            if(boltScr.isOn == false && boltOn)
            {
                Debug.Log("bolt temporarily disabled");
                eelHealth = eelHealth -1;
                boltOn = false;
            }
        }
        if((eelHealth == 0) && (!boltOn) && (eelDead == true))
        {
            animator.SetBool("isDead", true);
            eFOV.enabled = false;
            state = State.dead;
        }
    }

    void StunnedEel()
    {
        stunTime = Mathf.Clamp(stunTime,0,maxStunTime);
        stunTime -= Time.deltaTime;
        CapsuleCollider eelCollider = this.GetComponent<CapsuleCollider>();
        eelCollider.enabled = false;
        if(stunTime <= 0 && eelHealth > 0)
        {
            foreach(GameObject barnacle in barnacles)
            {
                barnacle.SetActive(true);
            }
            barnacleCount = barnacles.Length;
            boltScr.isOn = true;
            boltOn = true;
            boltScr.ResetBoltHealth();
            stunTime = 15;
            playerHid = true;
            eelCollider.enabled = true;
            animator.SetBool("isStunned", false);
            BGMManager.instance.SwitchBGMFade(13);
            BreathingManager.instance.SwitchBreathRate(0);
            state = State.patrolling;
        }
        else if(eelHealth == 0)
        {
            state = State.dead;
            deathObject.SetActive(true);
            eFOV.enabled = false;
            animator.SetBool("isDead", true);
        }
    }

    void StartAttacking()
    {
        state = State.attacking;
    }

    void Idle()
    {
        
    }

    private void HandleTailWiggleSpeed()
    {
        float velocity = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        animator.SetFloat("speed", velocity * 8);
    }

    private void GrowBarnacles()
    {
        foreach (GameObject barnacle in barnacles)
        {
            if(currentScale < maxScale && isGrowing)
            {
                currentScale += Time.deltaTime;
                barnacle.transform.localScale = new Vector3(2, currentScale * 2, 2);
            }
        }
    }

    public void StunTheEel()
    {
        barnacleCount--;

        if(barnacleCount <= 0)
        {
            animator.SetBool("isStunned", true);
            isGrowing = true;
            Invoke("GrowBarnacles",maxStunTime);
            CancelInvoke("StartAttacking");
            state = State.stunned;
        }
        else
        {
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(eelStinger);
            state = State.killedPlayer;
            BreathingManager.instance.StopBreathe();
            audioSource.PlayOneShot(eelSounds[0]);
            chaseSpeed = 0;
            patrolSpeed = 0;
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("Jumpscare");
            barnacleHolder.SetActive(false);
            boltSpark.SetActive(false);
        }
    }
}