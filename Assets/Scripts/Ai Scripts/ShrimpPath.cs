using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpPath : MonoBehaviour
{
    public GameObject shrimp;
    public GameObject[] pathPoints;
    public int numberOfPoints;
    public float speed;
    private Vector3 actualPosition;
    private int x;
    private bool isMoving;
    private bool isBlacklighted;

    private float timeBlacklighted = 0.5f;
    [SerializeField] private EatTheShrimp eatTheShrimp;
    void start()
    {
        x = 1;
    }
    
    void Update()
    {
        actualPosition = shrimp.transform.position;
        shrimp.transform.position = Vector3.MoveTowards(actualPosition, pathPoints[x].transform.position, speed * Time.deltaTime);
        float distance = Vector3.Distance(this.transform.position, pathPoints[x].transform.position);
        //Debug.Log(distance);
        
        if (isBlacklighted)
        {
            timeBlacklighted -= Time.deltaTime;
        } 
        if (timeBlacklighted <= 0f)
        {
            isBlacklighted = false;
            timeBlacklighted = 0.5f;
        }
        if (distance  < 0.5f && !isBlacklighted)
        {
            isMoving = false;
        }

        if(actualPosition == pathPoints[x].transform.position&& x != numberOfPoints -1 && isMoving) 
        {
            x++;
        }
    }
    public void MoveShrimp()
    {
        isMoving = true;
        isBlacklighted = true;
    }
    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag ("Enemy"))
        {
          eatTheShrimp.MakeMove();
          Debug.Log("Hewwo");
        }
        
    
    } 

}
