using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zooplankton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ZooplankotCount zooplankotCount = other.GetComponent<ZooplankotCount>();
        if (zooplankotCount !=null)
        {
            zooplankotCount.ZooplanktonCollected();
            gameObject.SetActive(false);
        }
    }
    
}

