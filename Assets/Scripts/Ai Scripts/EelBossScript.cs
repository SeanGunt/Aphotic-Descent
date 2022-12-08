using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EelBossScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathObject;
    [SerializeField] private float eelSpeed;
    [SerializeField] private float eelDashSpeed;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private GameObject playerDiver;

    //phase 1 vars
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
    sightBasedEnemyAi siBaAi;
    private float eelRange;
    PlayerHealthController pHC;
    InvisibilityMechanic pIM;
    public NavMeshAgent eelAgent;
    Vector3 destination;
    private float playerDistance;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip eelHiss;
    private float timer;

    private State state;
    public enum State
    {
        Phase1, Phase2, EB_hiding, EB_attacking
    }


    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        cc = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        eelAgent = GetComponent<NavMeshAgent>();
        eelAgent.speed = eelSpeed;
        eelAgent.updateRotation = true;
        eelAgent.autoBraking = false;
        eelAgent.acceleration = eelAgent.acceleration;
        eelAgent.angularSpeed = eelAgent.angularSpeed;
        timer = Random.Range(5f,10f);

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Debug.Log("player found");
            pHC = player.GetComponent<PlayerHealthController>();
            pIM = player.GetComponent<InvisibilityMechanic>();

            gen1Scr = gen1.GetComponent<generatorScript>();
            gen2Scr = gen2.GetComponent<generatorScript>();
            gen3Scr = gen3.GetComponent<generatorScript>();

            //bAiScr = this.GetComponent<BasicEnemyAi>();
            siBaAi = this.GetComponent<sightBasedEnemyAi>();
            
        }
        else
        {
            Debug.LogWarning("player not Found");
        }

        //CURRENTLY TESTING:
        //Phase 1
        //be sure to change when testing later

        //state = State.Phase2;
        state = State.Phase1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !eelDead)
        {
            audioSource.PlayOneShot(eelHiss);
            timer = Random.Range(5f,10f);
        }
        playerDistance = (player.transform.position-this.transform.position).sqrMagnitude;

        switch (state)
        {
            default:
            case State.Phase1:
                    phase1();
            break;
            case State.Phase2:
                    phase2();
            break;
            case State.EB_attacking:
                    eelAttacking();
            break;
        }
    }

    void phase2()
    {
        eelAgent.destination = player.transform.position;
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
            eelAgent.speed = 0;
            siBaAi.enabled = false;
        }
    }

    void eelAttacking()
    {
        /*
        nope
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            eelAgent.speed = 0;
            playerDiver.SetActive(false);
            mainCam.SetActive(false);
            jumpscareCam.SetActive(true);
            animator.SetTrigger("Jumpscare");
        }
    }
}
