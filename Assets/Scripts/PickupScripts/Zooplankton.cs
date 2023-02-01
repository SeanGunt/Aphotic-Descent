using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zooplankton : MonoBehaviour
{
    public void CollectPlankton()
    {
        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");
        ZooplankotCount zooplankotCount = player.GetComponent<ZooplankotCount>();
        if (zooplankotCount !=null)
        {
            zooplankotCount.ZooplanktonCollected();
            gameObject.SetActive(false);
        }
    }
    
}

