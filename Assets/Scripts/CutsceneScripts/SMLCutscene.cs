using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMLCutscene : MonoBehaviour
{
    public static SMLCutscene instance;
    private GameObject player;
    [SerializeField] private GameObject sManLabCutscene;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject hud;
    [SerializeField] private AudioClip shrimpShot, glassShatter, scareSound;
    [SerializeField] private AudioSource audioSource;
    private flashlightMechanic flashlightmechanic;
    private WeaponController weaponController;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        flashlightmechanic = player.GetComponent<flashlightMechanic>();
        weaponController = player.GetComponent<WeaponController>();     
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    public void StartCutscene()
    {
        flashlightmechanic.enabled = false;
        weaponController.enabled = false;
        playerMovement.enabled = false;
        hud.SetActive(false);
        mainCamera.SetActive(false);
        sManLabCutscene.SetActive(true);
    }

    public void EndCutscene()
    {
        playerMovement.enabled = true;
        flashlightmechanic.enabled = true;
        weaponController.enabled = true;    
        mainCamera.SetActive(true);
        hud.SetActive(true);
        sManLabCutscene.SetActive(false);
    }

    public void PlaySound(int index)
    {
        switch (index)
        {
            default:
            case 1: 
                audioSource.PlayOneShot(shrimpShot);
                break;
            case 2:
                audioSource.PlayOneShot(glassShatter);
                break;
            case 3:
                audioSource.PlayOneShot(scareSound);
                break;

        }
    }
}