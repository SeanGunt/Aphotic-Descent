using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class psEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] travelPoints;
    [SerializeField] private GameObject thePlayer;
    [SerializeField] private GameObject psGunAimer;
    [SerializeField] private Vector3 playerLocation;
    [SerializeField] private float moveTimer = 14.0f;
    [SerializeField] private float shootTimer = 2.0f;
    [SerializeField] public float distBtwn;
    [SerializeField] private float gunRange;
    [SerializeField] public int currentPoint = 0;
    public NavMeshAgent psAgent;
    private bool nextPoint = true;
    [SerializeField] private bool inShootRange = false;
    private float moveTimerReset;
    private float shootTimerReset;
    private float resetSpeed;
    public int aimWhere;
    psGunRotate gunRotating;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        psAgent = GetComponent<NavMeshAgent>();
        psAgent.destination = travelPoints[currentPoint].position;

        gunRotating = GetComponentInChildren<psGunRotate>();
        if(gunRotating != null)
        {
            Debug.Log("gun found");
        }

        moveTimerReset = moveTimer;

        shootTimerReset = shootTimer;

        resetSpeed = psAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        //playerLocation = thePlayer.transform.position;

        //distBtwn = Vector3.Distance(playerLocation, this.transform.position);

        if(nextPoint)
        {
            moveToPoint();
        }

        if((!psAgent.pathPending && psAgent.remainingDistance < 0.5f))
        {
            moveTimer -= Time.deltaTime;
            if(moveTimer <= 0 && !inShootRange)
            {
                nextPoint = true;
                moveTimer = moveTimerReset;
            }
        }

        if(distBtwn <= gunRange)
        {
            psAgent.speed = 0;
            stopAndAim();
        }
        else
        {
            psAgent.speed = resetSpeed;
            inShootRange = false;
            gunRotating.rotateToTarget = false;
        }

        if(inShootRange)
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer <= 0)
            {
                gunRotating.shoot();
                shootTimer = shootTimerReset;    
            }
        }
        else
        {
            shootTimer = shootTimerReset;
        }
    }

    void moveToPoint()
    {
        nextPoint = false;
        psAgent.destination = travelPoints[currentPoint+1].position;
    }

    public void stopAndAim()
    {
        inShootRange = true;
        if(aimWhere == 0)
        {
            gunRotating.rotateToTarget = true;
        }
        else
        {
            gunRotating.rotateToTarget = false;
        }
    }
}
