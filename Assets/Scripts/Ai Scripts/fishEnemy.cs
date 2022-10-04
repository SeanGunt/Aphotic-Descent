using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEnemy : MonoBehaviour
{

    private Vector3 startPos;

    public Transform position1;
    public Transform position2;

    //How fast the enemy is when chasing the player
    public float chaseSpeed = 2.0F;

    //For how long the lerp is going to take, among other things
    public float patrolSpeed = 2.0F;

    private GameObject player;

    private float beginningTime;

    private float totalLength;

    public bool backToStart = false;


    //Detection booleans
    //public bool triggered = false;
    //public bool isPatrolling = true;

    //Countdown timer for patrol reset
    //private float restartPatrol = 2.0f;
    //private float resetTimer;

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Time.time is time since the start of the application
        beginningTime = Time.time;

        //'From which point to what point'
        totalLength = Vector3.Distance(position1.position, position2.position);

        startPos = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(backToStart != false)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }


        /*
        if(isPatrolling == true)
        {
            //Patrolling2();
        }
        */


    }

    private void Patrolling()
    {

        //Next three lines of code required for calculation of Lerp and PingPong movement
        float totalDist = (Time.time - beginningTime) * patrolSpeed;

        float divTotal = totalDist / totalLength;

        transform.position = Vector3.Lerp(position1.transform.position, position2.transform.position, Mathf.PingPong(divTotal, 1));

        //Debug.Log("patrolling");



        /*
        if(isPatrolling == false)
        {

            //After chasing, go back to start position, wait for a moment, then return to patrolling

            transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, patrolSpeed * Time.deltaTime);
            
            restartPatrol -= Time.deltaTime;
            
            if(restartPatrol <= 0)
            {
                isPatrolling = true;
            }
            Debug.Log("not patrolling");
        }
        */

        //Move to starting position
        //transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, patrolSpeed * Time.deltaTime);    


    }



    /*
    //The Lerp and PingPong Movement code
    private void Patrolling2()
    {
        float totalDist = (Time.time - beginningTime) * patrolSpeed;

        float divTotal = totalDist / totalLength;

        transform.position = Vector3.Lerp(position1.transform.position, position2.transform.position, Mathf.PingPong(divTotal, 1));

        Debug.Log("patrolling");
    }
    */


    //Moves toward the Player
    private void Chasing()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        //Debug.log("chasing");
    }
}