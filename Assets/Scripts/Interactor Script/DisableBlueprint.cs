using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisableBlueprint : MonoBehaviour
{
    private GameObject player;
    private PlayerInput playerInput;
    private PauseControls pauseControls;
   private void Awake()
   {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseControls = player.GetComponent<PauseControls>();
        playerInput = player.GetComponent<PlayerInput>();

        
   }
    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (playerInput.actions["Interact"].triggered)
            {
                pauseControls.paused = false;
                Time.timeScale = 1f;
                this.gameObject.SetActive(false);
            }
        }
    }
}
