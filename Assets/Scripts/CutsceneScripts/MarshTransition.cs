using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshTransition : MonoBehaviour
{
    public static MarshTransition instance;
    private GameObject player;
    [SerializeField] private GameObject playerVisual, shrimpMan1, shrimpMan2, flashlightLight, marshCamScene;

    [SerializeField] private GameObject cutsceneShrimp, rockObj;
    [SerializeField] private flashlightMechanic flashlightController;
    [SerializeField] private AudioListener marshCamAudioListener;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PauseControls pauseControls;

    private void Awake()
    {
        instance = this;
    }

    public void StartCutscene()
    {
        playerVisual.SetActive(false);
        flashlightController.enabled = false;
        flashlightLight.SetActive(false);
        weaponController.enabled = false;
        playerMovement.enabled = false;
        pauseControls.enabled = false;
        //mainCamera.SetActive(false);
        shrimpMan1.SetActive(false);

        cutsceneShrimp.SetActive(true);

        marshCamScene.SetActive(true);
    }

    public void EndCutscene()
    {
        //mainCamera.SetActive(true);
        marshCamScene.SetActive(false);
        marshCamAudioListener.enabled = false;
        
        //playerVisual.SetActive(true);

        cutsceneShrimp.SetActive(false);
        rockObj.SetActive(false);
        flashlightController.enabled = true;
        flashlightLight.SetActive(true);
        weaponController.enabled = true;
        playerMovement.enabled = true;
        pauseControls.enabled = true;
        shrimpMan2.SetActive(true);

        
    }
}
