using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluprintInteract : MonoBehaviour
{
   private GameObject player;
   private PauseControls pauseControls;
   private void Awake()
   {
     player = GameObject.FindGameObjectWithTag("Player");
        pauseControls = player.GetComponent<PauseControls>();
   }
   public void Pickup()
   {
    pauseControls.paused = true;
    Time.timeScale = 0;
    
   }

}
