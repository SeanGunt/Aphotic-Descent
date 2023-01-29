using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class psEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] travelPoints;
    [SerializeField] private GameObject thePlayer;
    [SerializeField] private GameObject psGunAimer;
    [SerializeField] private GameObject psGunHead;
    [SerializeField] private Vector3 playerLocation;
    [SerializeField] private float moveTimer = 15.0f;
    [SerializeField] private float timerReset;
    [SerializeField] private float distBtwn;
    public NavMeshAgent psAgent;
    private bool unchosen = false;
    private float resetSpeed;

    // Start is called before the first frame update
    void Start()
    {
        psAgent = GetComponent<NavMeshAgent>();

        timerReset = moveTimer;

        resetSpeed = psAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        playerLocation = thePlayer.transform.position;

        distBtwn = Vector3.Distance(playerLocation, this.transform.position);
        Debug.Log(distBtwn + " is the dist btwn");
        
        if(unchosen == false && moveTimer >= 0)
        {
           Debug.Log("ps waiting");
           moveTimer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("ps beginning movement");
            unchosen = true;
            moveToPoint();
        }

        if(distBtwn <= 15.0f)
        {
            psAgent.speed = 0;
            stopAndShoot();
        }
        else
        {
            psAgent.speed = resetSpeed;
        }
    }

    void moveToPoint()
    {
        if(moveTimer <= 0 && unchosen)
        {
            Debug.Log("ps going somewhere");
            psAgent.destination = travelPoints[Random.Range(0, travelPoints.Length)].position;

            if(!psAgent.pathPending || psAgent.remainingDistance < 0.5f)
            {
                moveTimer = timerReset;
                unchosen = false;
            }
        }
    }

    void stopAndShoot()
    {
        //the plan is to rotate the gunAimer at the player when this is active
        //then check if the gunRayCast is detecting the player, and if so, shoot
        //else stand and wait
    }
}
