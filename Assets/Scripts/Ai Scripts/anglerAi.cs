using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class anglerAi : MonoBehaviour
{
    private int currentPoint = 0;
    public NavMeshAgent anglerAgent;
    private bool unchosen;
    public State state;
    public enum State
    {
        patrolling, dead
    }
    [SerializeField] private Transform[] patrolPoints;

    // Start is called before the first frame update
    void OnAwake()
    {
        state = State.patrolling;
        anglerAgent = this.GetComponent<NavMeshAgent>();

        anglerAgent.destination = patrolPoints[currentPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.patrolling:
                patrolling();
            break;
            case State.dead:
                Debug.Log("angler dead");
                anglerAgent.speed = 0;
                anglerAgent.acceleration = 0;
            break;
        }
    }

    void patrolling()
    {
        if(!anglerAgent.pathPending && anglerAgent.remainingDistance < 0.5f)
        {
            //Debug.Log("arrived at point");
            
            unchosen = true;
        }
        
        if(unchosen)
        {
            //Debug.Log("next point");

            anglerAgent.destination = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
            unchosen = false;
        }
    }
}
