using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudMarshCutscene : MonoBehaviour
{
    public static MudMarshCutscene instance;
    private PlayerMovement playerMovement;
    private GameObject player;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject caveCollapseCamera;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject cutsceneObj;
    [SerializeField] private GameObject zooplanktonCutscene;
    private flashlightMechanic flashlightmechanic;
    private WeaponController weaponController;
    [SerializeField] private GameObject flashlightLight;
    [HideInInspector] public bool inMarshCutscene;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        flashlightmechanic = player.GetComponent<flashlightMechanic>();
        weaponController = player.GetComponent<WeaponController>();
    }

    public void StartCutscene()
    {
        inMarshCutscene = true;
        hud.SetActive(false);
        cutsceneObj.SetActive(true);
        playerMovement.enabled = false;
        flashlightmechanic.enabled = false;
        flashlightLight.SetActive(false);
        weaponController.enabled = false;
        playerVisual.SetActive(false);
        mainCamera.SetActive(false);
        caveCollapseCamera.SetActive(true);
    }

    public void EndCutscene()
    {
        caveCollapseCamera.SetActive(false);
        zooplanktonCutscene.SetActive(true);
        BGMManager.instance.SwitchBGMFade(15);
    }
}
