using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaseController : MonoBehaviour
{

    //holds the fishEnemies in their entirety as references
    //as such, it allows for referencing their component scripts and variables
    //if you wanted, you could have multiple fishEnemies activated by the same trigger
    public fishEnemy[] fishArray;


    //MAKE SURE PLAYER OBJECT IS TAGGED AS PLAYER
    //when the player hits the trigger box, it activates the chasing script
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //make sure the array name is the same as the script in question (fishArray in this case)
            //will error because it 'doesn't exist' otherwise
            foreach(fishEnemy enemy in fishArray)
            {
                enemy.backToStart = true;
            }
        }
    }

    //when player leaves trigger box, deactivates chase script, activates patrol script
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