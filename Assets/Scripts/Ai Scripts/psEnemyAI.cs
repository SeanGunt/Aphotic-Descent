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
    [SerializeField] private float shootTimer = 3.0f;
    [SerializeField] private float distBtwn;
    [SerializeField] private float closestPoint;
    [SerializeField] private float gunRange;
    public NavMeshAgent psAgent;
    private bool unchosen = true;
    private bool inShootRange = false;
    private float moveTimerReset;
    private float shootTimerReset;
    private float resetSpeed;
    psGunRotate gunRotating;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        psAgent = GetComponent<NavMeshAgent>();

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
        playerLocation = thePlayer.transform.position;

        distBtwn = Vector3.Distance(playerLocation, this.transform.position);
        //Debug.Log(distBtwn + " is the dist btwn");
        //Debug.Log("remaining dist is: " + psAgent.remainingDistance);
        //Debug.Log("pathpending is: " + psAgent.pathPending);

        if(unchosen)
        {
            //currently the same as the freakfish
            //if you see this I've forgotten to remove the comments

            moveToPoint();
        }

        if((!psAgent.pathPending && psAgent.remainingDistance < 0.5f))
        {
            moveTimer -= Time.deltaTime;
            if(moveTimer <= 0 && !inShootRange)
            {
                unchosen = true;
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
            gunRotating.rotateToPlayer = false;
        }

        if(inShootRange)
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer <= 0)
            {
                Debug.Log("this is the part where it kills you");
                shoot();
                shootTimer = shootTimerReset;    
            }
        }
        else
        {
            shootTimer = shootTimerReset;
        }

        shoot();
    }

    void moveToPoint()
    {
        Debug.Log("ps moving to new point");

        unchosen = false;
        psAgent.destination = travelPoints[Random.Range(0, travelPoints.Length)].position;
    }

    void stopAndAim()
    {
        //the plan is to rotate the gunAimer at the player when this is active
        //then check if gunRayCast is detecting the player, and if so, shoot
        //else stand and wait?
        Debug.Log("hey in range");
        inShootRange = true;

        gunRotating.rotateToPlayer = true;


    }

    void shoot()
    {
        //uses raycast
        Vector3 centerRay = transform.TransformDirection(Vector3.forward) * gunRange;
        Debug.DrawRay(psGunHead.transform.position, centerRay, Color.red);

        if(Physics.Raycast(psGunHead.transform.position, centerRay, out hit, gunRange))
        {
            Debug.Log(hit.collider.gameObject.name + " was hit");
        }
    }

    /*
    void findClosestPoint()
    {
        closestPoint = Mathf.Infinity;

        foreach(Transform currentPoint in travelPoints)
        {
            closestPoint = (currentPoint.position - thePlayer.transform.position).sqrMagnitude;
        }
    }
    */
}
