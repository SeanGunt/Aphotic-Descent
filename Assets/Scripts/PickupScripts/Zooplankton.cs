using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zooplankton : MonoBehaviour
{
    public void CollectPlankton()
    {
        GameDataHolder.numOfZooplanktonCollected++;
        this.gameObject.SetActive(false);
    }
}

