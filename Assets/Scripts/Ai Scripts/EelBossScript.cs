using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EelBossScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float eelSpeed;
    [SerializeField] private float eelDashSpeed;
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
        }
        else
        {
            Debug.LogWarning("player not Found");
        }


        //CURRENTLY TESTING:
        //Phase 2
        //be sure to change when testing later

        state = State.Phase2;
    }

    // Update is called once per frame
    void Update()
    {

        playerDistance = (player.transform.position-this.transform.position).sqrMagnitude;

        switch (state)
        {
            default:
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

    void eelAttacking()
    {
        /*

        */
    }
}
