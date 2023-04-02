using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFCutscene : MonoBehaviour
{
    public static FFCutscene instance;
    private GameObject player;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject freakFish;
    [SerializeField] private GameObject freakFishCutsene;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject hud;
    private PlayerMovement playerMovement;
    private ffScr freakFishScript;
    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        freakFishScript = freakFish.GetComponent<ffScr>();
    }

    public void StartCutscene()
    {
        playerVisual.SetActive(false);
        playerMovement.enabled = false;
        freakFishScript.enabled = false;
        hud.SetActive(false);
        mainCamera.SetActive(false);
        freakFishCutsene.SetActive(true);
    }

    public void EndCutscene()
    {
        playerVisual.SetActive(true);
        playerMovement.enabled = true;
        freakFishScript.enabled = true;
        mainCamera.SetActive(true);
        hud.SetActive(true);
        freakFishCutsene.SetActive(false);
    }
}