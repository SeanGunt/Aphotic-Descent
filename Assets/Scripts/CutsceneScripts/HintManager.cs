using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField]private GameObject cameraHintCutscene;
    [SerializeField]private Camera hintCam;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField]private GameObject mainCamera;
    [SerializeField]private Image fadeToBlackImage;
    [SerializeField]private GameObject hud;

    private void Awake()
    {
        if (GameDataHolder.eelIsDead && GameDataHolder.inEelCave && !GameDataHolder.hintCamPlayed)
        {
            cameraHintCutscene.SetActive(true);
            playerMovement.inCutscene = true;
            hud.SetActive(false);
            Invoke("EndCutscene", 3.01f);
        }
    }

    private void EndCutscene()
    {
        hintCam.enabled = false;
        playerMovement.inCutscene = false;
        mainCamera.SetActive(true);
        hud.SetActive(true);
        cameraHintCutscene.SetActive(false);
    }
}
