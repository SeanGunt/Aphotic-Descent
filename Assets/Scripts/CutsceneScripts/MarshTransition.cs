using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshTransition : MonoBehaviour
{
    public static MarshTransition instance;
    private GameObject player;
    [SerializeField] private GameObject playerVisual, mainCamera, shrimpMan1, shrimpMan2, flashlightLight, marshCamScene;
    [SerializeField] private flashlightMechanic flashlightController;
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
        mainCamera.SetActive(false);
        shrimpMan1.SetActive(false);
        marshCamScene.SetActive(true);
    }

    public void EndCutscene()
    {
        mainCamera.SetActive(true);
        marshCamScene.SetActive(false);
        playerVisual.SetActive(true);
        flashlightController.enabled = true;
        flashlightLight.SetActive(true);
        weaponController.enabled = true;
        playerMovement.enabled = true;
        pauseControls.enabled = true;
        shrimpMan2.SetActive(true);
    }
}
