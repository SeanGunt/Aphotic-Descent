using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaseController : MonoBehaviour
{
    public fishEnemy[] fishArray;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach(fishEnemy enemy in fishArray)
            {
                enemy.backToStart = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach(fishEnemy enemy in fishArray)
            {
                enemy.backToStart = false;
                //enemy.isPatrolling = false;
            }
        }
    }
}