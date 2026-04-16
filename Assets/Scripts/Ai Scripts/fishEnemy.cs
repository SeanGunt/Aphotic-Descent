using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class fishEnemy : MonoBehaviour
{
    public Transform[] patrolPositions;
    private int positionInPoints;
    public float chaseSpeed = 2.0F;
    public float patrolSpeed = 2.0F;
    public float chaseSpeed2 = 12.5f;
    public float patrolSpeed2 = .8f;
    [SerializeField]private GameObject playerDiver, player, playerHead, mainCam, jumpscareCam, deathObject, deathCube, barnacleHolder, deadEel;
    [SerializeField]private GameObject[] barnacles;
    [SerializeField]private float currentScale, maxScale, trackingCooldown, stunTime;
    public float StunTime { get { return stunTime; } }
    private float maxStunTime, maxTrackingCooldown;
    [SerializeField]private Animator animator;
    public Animator FrankyAnimator { get { return animator; } }
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
    //for the cooldown after a hit.
    private bool isCoolingDown;
    private int eelHealth = 4;
    [SerializeField]private int barnacleCount;
    //[SerializeField] private int phase;
    private bool eelPanicking = false;
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
    //private CapsuleCollider eelColider;
    Vector3 lastPosition;

    private enum State
    {
        attacking, patrolling, dead, killedPlayer, stunned, idle, transitioning, lockingOn
    }
    
    void Awake()
    {
		//eelCollider = this.GetComponent<CapsuleCollider>();
		randomTime = Random.Range(4f,9f);
        audioSource = this.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        cc = GetComponent<CapsuleCollider>();
        beginningTime = Time.time;
        eFOV = this.GetComponent<enemyFieldOfView>();
        animator = GetComponentInChildren<Animator>();
        //phase = 1;
        currentScale = 1;
        maxScale = 6;
        eelPanicking = false;
        positionInPoints = 0;
        maxTrackingCooldown = trackingCooldown;
        maxStunTime = stunTime;
        //barnacleCount = 6;
        //isGrowing = false;
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
                Phase1();
                break;
            case State.attacking:
                Attacking();
                Phase1();
                break;
            case State.killedPlayer:
                break;
            case State.transitioning:
                //GrowBarnacles();
                break;
            case State.stunned:
                StunnedEel();
                Phase1();
                //Phase2();
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
        
        if (totalLength <= 7.5f)
        {
            movingToNextPosition = false;
        }

        if (eFOV.canSeePlayer && !iM.isSafe && !playerHid)
        {
            if (eelHealth > 1)
            {
				BGMManager.instance.SwitchBGM(4);
			}
            BreathingManager.instance.SwitchBreathRate(2);
            audioSource.PlayOneShot(eelSounds[3]);
            state = State.attacking;
        }
        
        trackingCooldown = Mathf.Clamp(trackingCooldown, 0, maxTrackingCooldown);
        if (playerHid && !isCoolingDown)
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
            if (!eelPanicking)
            {
                BGMManager.instance.SwitchBGM(3);
            }
            playerHid = true;
        }
    }

    private void Dead()
    {
        cc.enabled = false;
        eFOV.enabled = false;
        eelDead = true;
        //GameDataHolder.eelIsDead = true;
		GameDataHolder.eelIsDead = true;
		GameDataHolder.eelFound = true;
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

        if ((eelHealth == 1 && eelDead == false && eelPanicking == false))
        {
            eelPanicking = true;
			BGMManager.instance.SwitchBGMFade(13);
			objectiveUpdater.SixthObjective();
			chaseSpeed = chaseSpeed2;
			patrolSpeed = patrolSpeed2;
		}
        
        if((eelHealth == 0) && (!g1On && !g2On && !g3On && !g4On) && (eelDead == false))
        {
            BGMManager.instance.SwitchBGMFade(12);
            animator.SetBool("isDead", true);
            deathObject.SetActive(true);
            eFOV.enabled = false;
            state = State.dead;
            //audioSource.PlayOneShot(eelSounds[4]);
        }
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
            stunTime = 15;
            playerHid = true;
            eelCollider.enabled = true;
            eFOV.enabled = true;
            animator.SetBool("isStunned", false);
            if (!eelPanicking)
            {
                BGMManager.instance.SwitchBGM(3);

			}
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

    void Idle()
    {
        //Invoke("ResumePatrol", 7.5f);
    }

    private void HandleTailWiggleSpeed()
    {
        float velocity = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        animator.SetFloat("speed", velocity * 8);
    }

    public void StunTheEel()
    {
        audioSource.PlayOneShot(eelSounds[5]);
		animator.SetBool("isStunned", true);
		//isGrowing = true;
		OverrideCooldown();
		//CancelInvoke("StartAttacking");
		state = State.stunned;
	}

    private void ResumePatrol()
    {
        if (isCoolingDown)
        {
			isCoolingDown = false;
			state = State.patrolling;
			this.GetComponent<CapsuleCollider>().enabled = true;
			eFOV.enabled = true;
			//playerHid = true;
		}   
	}

    private void ResumeDetection()
    {
        if (isCoolingDown)
        {
            isCoolingDown = false;
            this.GetComponent<CapsuleCollider>().enabled = true;
            eFOV.enabled = true;
            //playerHid = true;
        }
    }

	public void OverrideCooldown()
	{
		isCoolingDown = false;
		//cc.enabled = true;
		CancelInvoke("ResumeDetection");


		//state = State.attacking;
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


			//pHC.ChangeHealth(-11.5f);
			//pHC.TakeDamage();
			//pHC.isBleeding = true;
			//         if (pHC.playerHealth <= 0)
			//         {
			//	pHC.playerHealth = pHC.maxHealth;
			//	audioSource.PlayOneShot(eelStinger);
			//	state = State.killedPlayer;
			//	BreathingManager.instance.StopBreathe();
			//	audioSource.PlayOneShot(eelSounds[0]);
			//	chaseSpeed = 0;
			//	patrolSpeed = 0;
			//	playerDiver.SetActive(false);
			//	mainCam.SetActive(false);
			//	jumpscareCam.SetActive(true);
			//	animator.SetTrigger("Jumpscare");
			//	barnacleHolder.SetActive(false);
			//	boltSpark.SetActive(false);
			//}
			//         else
			//         {
			//             isCoolingDown = true;
			//             //state = State.idle;
			//             playerHid = true;
			//	cc.enabled = false;
			//             eFOV.canSeePlayer = false;
			//	eFOV.enabled = false;
			//             //state = State.patrolling;
			//	Invoke("ResumeDetection", 1.5f);
			//}
		}
	}
}