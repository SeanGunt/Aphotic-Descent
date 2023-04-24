using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DisableLore : MonoBehaviour
{
    private PauseControls pauseControls;
    private PlayerInput playerInput;
    private GameObject player;
    private Volume volume;
    private GameObject volumeObj;
    [SerializeField] private GameObject basicTextObj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string enylopediaText;
    private Text addedLoreToEncyclopediaText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        pauseControls = player.GetComponent<PauseControls>();
        volumeObj =  GameObject.FindGameObjectWithTag("PostProcMain");
        volume = volumeObj.GetComponent<Volume>();
        addedLoreToEncyclopediaText = basicTextObj.GetComponent<Text>();
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (playerInput.actions["Interact"].triggered)
            {
                DepthOfField depthOfField;
                if (volume.profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                }
                LogPickup.logPickedUp = false;
                audioSource.Stop();
                Time.timeScale = 1f;
                pauseControls.paused = false;
                addedLoreToEncyclopediaText.text = enylopediaText;
                basicTextObj.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
