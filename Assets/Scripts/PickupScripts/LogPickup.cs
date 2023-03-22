using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LogPickup : MonoBehaviour
{
    private PauseControls pauseControls;
    private GameObject player;
    public static bool logPickedUp;
    private Volume volume;
    private GameObject volumeObj;
    [SerializeField] private GameObject objectiveText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseControls = player.GetComponent<PauseControls>();
        volumeObj =  GameObject.FindGameObjectWithTag("PostProcMain");
        volume = volumeObj.GetComponent<Volume>();
    }
    public void OnPickup()
    {
        DepthOfField depthOfField;
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            depthOfField.active = true;
        }
        pauseControls.paused = true;
        logPickedUp = true;
        objectiveText.SetActive(false);
        Time.timeScale = 0;
    }
}
