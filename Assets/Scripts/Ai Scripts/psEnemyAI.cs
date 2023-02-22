using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class psEnemyAI : MonoBehaviour
{
     [SerializeField] private Transform[] travelPoints;
    [SerializeField] public GameObject thePlayer;
    [SerializeField] private Vector3 playerLocation;
    [SerializeField] private float moveTimer = 14.0f;
    [SerializeField] private float shootTimer = 2.0f;
    [SerializeField] private float gunRange;
    [SerializeField] private float aimRange;
    [SerializeField] private GameObject psGunAimer;
    [SerializeField] private GameObject psGunHead;
    [SerializeField] private float gunDamage;
    [SerializeField] private LayerMask doNotIgnoreLayer;
    [SerializeField] private bool inShootRange = false;
    [SerializeField] public float distBtwn;
    [SerializeField] public int currentPoint = 0;
    [SerializeField] public int nPoint;
    [SerializeField] public GameObject psTarget;
    [SerializeField] public GameObject psRotationPoint;
    private bool nextPoint = true;
    private float moveTimerReset;
    private float shootTimerReset;
    private float resetSpeed;
    private Vector3 startPos;
    public NavMeshAgent psAgent;
    public int aimWhere;
    public bool rotateToTarget;
    RaycastHit hit;
    RaycastHit hit2;


    // Start is called before the first frame update
    void Start()
    {
        psAgent = GetComponent<NavMeshAgent>();
        psAgent.destination = travelPoints[currentPoint].position;

        moveTimerReset = moveTimer;

        shootTimerReset = shootTimer;

        resetSpeed = psAgent.speed;

        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerLocation = thePlayer.transform.position;

        distBtwn = Vector3.Distance(playerLocation, this.transform.position);

        nPoint = currentPoint+1;

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

        if(distBtwn <= aimRange)
        {
            psAgent.speed = 0;
            stopAndAim();
        }
        else
        {
            psAgent.speed = resetSpeed;
            inShootRange = false;
            rotateToTarget = false;
        }

        if(inShootRange)
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer <= 0)
            {
                shoot();
                shootTimer = shootTimerReset;    
            }
        }
        else
        {
            shootTimer = shootTimerReset;
        }

        if(rotateToTarget)
        {
            Vector3 targetDirection = psTarget.transform.position - transform.position;
            psGunAimer.transform.LookAt(psTarget.transform.position);
            tarRot(psTarget.transform);
        }
        else
        {
            psGunAimer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void moveToPoint()
    {
        if(travelPoints[nPoint] != null)
        {
            psAgent.destination = travelPoints[nPoint].position;
            nextPoint = false;
        }
        else
        {
            return;
        }
    }

    public void stopAndAim()
    {
        inShootRange = true;
        if(aimWhere == 0)
        {
            psTarget = thePlayer;
            rotateToTarget = true;
        }
        else
        {
            psTarget = null;
            rotateToTarget = false;
        }
    }

    public void tarRot(Transform target)
    {
        Vector3 restTarget = psTarget.transform.position;
        restTarget.y = psRotationPoint.transform.position.y;
        transform.LookAt(restTarget);
    }

    public void shoot()
    {
        //uses raycast
        Vector3 centerRay = transform.TransformDirection(Vector3.forward) * gunRange;
        Debug.DrawRay(psGunHead.transform.position, centerRay, Color.red);

        if(Physics.Raycast(psGunHead.transform.position, centerRay, out hit2, gunRange, doNotIgnoreLayer))
        {
            Debug.Log(hit.collider.gameObject.name + " was hit");

            if(hit.collider.gameObject == thePlayer)
            {
                //pHC.ChangeHealth(-(pHC.maxHealth / gunDamage)); //essentially, 'what fraction of health (based on total maxHealth) will the gun remove from the player?'
                                                                  //maxHealth is 15.0 atm, lower number for damage means more health is taken away
                Debug.Log("hit player");
            }

            if(hit.collider.gameObject.tag == "psBomb" || hit.collider.gameObject.tag == "psLamp")
            {
                //hit.collider.gameObject.GetComponent<psShootObjects>().subtractHealth();
            }
        }
    }
}
