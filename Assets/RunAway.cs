using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : MonoBehaviour
{
    private NavMeshAgent _agent;
    public GameObject Player;

    public float enemyDistanceRun = 4.0f;

    void Start()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance < enemyDistanceRun)
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            _agent.SetDestination(newPos);
        }
    }
}
