using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTheShrimp : MonoBehaviour
{
    public GameObject crab;
    public GameObject[] pathPoints;
    public int numberOfPoints;
    public float speed;
    public GameObject yummyShrimp;
    public GameObject shrimpCollider;
    private Vector3 actualPosition;
     private int x;
     
      void start()
    {
        x = 1;
    }

   
     void hermitMove()
     {
          actualPosition = crab.transform.position;
            crab.transform.position = Vector3.MoveTowards(actualPosition, pathPoints[x].transform.position, speed * Time.deltaTime);

        if(actualPosition == pathPoints[x].transform.position&& x != numberOfPoints -1) 
        {
            x++;
        }

     }
      
    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag ("Enemy"))
        {
            GameObject.Destroy(yummyShrimp);
            hermitMove();
           
        }
        
    
    } 
    }


