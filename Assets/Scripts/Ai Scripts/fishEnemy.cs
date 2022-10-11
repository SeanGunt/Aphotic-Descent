using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEnemy : MonoBehaviour
{

    [SerializeField] private Transform moveObj;

    public Transform position1;
    public Transform position2;

    public float chaseSpeed = 2.0F;

    public float patrolSpeed = 2.0F;

    private GameObject player;

    private float beginningTime;

    private float totalLength;

    public bool backToStart = false;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        beginningTime = Time.time;

        totalLength = Vector3.Distance(position1.position, position2.position);

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

    }

    private void Patrolling()
    {

        //Next three lines of code required for calculation of Lerp and PingPong movement
        float totalDist = (Time.time - beginningTime) * patrolSpeed;

        float divTotal = totalDist / totalLength;

        moveObj.transform.position = Vector3.Lerp(position1.transform.position, position2.transform.position, Mathf.PingPong(divTotal, 1));

        //Debug.Log("patrolling");

        if(Mathf.FloorToInt(divTotal)%2 != 0)
        {
            transform.LookAt(position1);
        }
        else
        {
            transform.LookAt(position2);
        }

        this.transform.position = Vector3.MoveTowards(transform.position, moveObj.transform.position, patrolSpeed * Time.deltaTime);

    }

    //Moves toward the Player
    private void Chasing()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        transform.LookAt(player.transform);

        //Debug.log("chasing");
    }
}