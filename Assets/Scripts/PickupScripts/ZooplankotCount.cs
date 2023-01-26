using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZooplankotCount : MonoBehaviour
{
    public int NumberOfZooplankton { get; private set; }
   
   public UnityEvent<ZooplankotCount> OnZooplanktonCollected;
    public void ZooplanktonCollected()
    {
        NumberOfZooplankton++;
        OnZooplanktonCollected.Invoke(this);
    }
}
