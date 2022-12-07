using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EelBossScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float eelSpeed;
    [SerializeField] private float eelDashSpeed;

    //phase 1 vars
    [SerializeField] private GameObject gen1;
    [SerializeField] private GameObject gen2;
    [SerializeField] private GameObject gen3;
    generatorScript gen1Scr;
    generatorScript gen2Scr;
    generatorScript gen3Scr;
    private bool g1On = true;
    private bool g2On = true;
    private bool g3On = true;
    private int eelHealth = 3;
    private bool eelDead = false;
    //BasicEnemyAi bAiScr;
    sightBasedEnemyAi siBaAi;

    private float eelRange;
    PlayerHealthController pHC;
    InvisibilityMechanic pIM;
    public NavMeshAgent eelAgent;
    Vector3 destination;
    private float playerDistance;

    private State state;
    public enum State
    {
        Phase1, Phase2, EB_hiding, EB_attacking
    }


    void Awake()
    {
        eelAgent = GetComponent<NavMeshAgent>();
        eelAgent.speed = eelSpeed;
        eelAgent.updateRotation = true;
        eelAgent.autoBraking = false;
        eelAgent.acceleration = 250;
        eelAgent.angularSpeed = 250;

        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Debug.LogWarning("player found");
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
            eelDead = true;
            Debug.Log("eel dead");
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
            pHC.ChangeHealth(-20.0f);
        }
    }
}
