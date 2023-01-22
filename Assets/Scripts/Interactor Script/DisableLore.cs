using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DisableLore : MonoBehaviour
{
    private PauseControls pauseControls;
    private PlayerInputActions playerInputActions;
    private GameObject player;
    private Volume volume;
    private GameObject volumeObj;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseControls = player.GetComponent<PauseControls>();
        volumeObj =  GameObject.FindGameObjectWithTag("PostProcMain");
        volume = volumeObj.GetComponent<Volume>();
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (playerInputActions.PlayerControls.Interact.triggered)
            {
                DepthOfField depthOfField;
                if (volume.profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                }
                LogPickup.logPickedUp = false;
                Time.timeScale = 1f;
                pauseControls.paused = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
