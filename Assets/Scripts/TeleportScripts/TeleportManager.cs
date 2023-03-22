using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviour
{
    private GameObject player, fogCube;
    [SerializeField] private Vector3 teleportPosition;
    private PlayerMovement playerMovement;
    private GameObject mainCamera;
    [SerializeField] private Image fadeToBlackImage;
    [SerializeField] private int teleportNumber;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fogCube = GameObject.FindGameObjectWithTag("FogCube");
        playerMovement = player.GetComponent<PlayerMovement>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void BasicTeleport()
    {
        StartCoroutine(Fade(1f));
    }

    private void CheckLocation()
    {
        if(teleportNumber == 1)
        {
            TeleportToKelpMaze();
        }

        if(teleportNumber == 2)
        {
            TelpeortToLab();
        }

        if(teleportNumber == 3)
        {
            TeleportToRidge();
        }

        if (teleportNumber == 4)
        {
            TeleportToEelCave();
        }
    }

    private void TeleportToKelpMaze()
    {
        GameDataHolder.inSub = false;
        GameDataHolder.inKelpMaze = true;
        GameDataHolder.inLab = false;
        GameDataHolder.inEelCave = false;
        playerMovement.inCutscene = true;
        mainCamera.SetActive(false);
        BGMManager.instance.StopMusic();
    }

    private void TeleportToRidge()
    {
        GameDataHolder.inLab = false;
        GameDataHolder.inKelpMaze = true;
        BGMManager.instance.SwitchBGMFade(0);
    }

    private void TelpeortToLab()
    {
        GameDataHolder.inLab = true;
        GameDataHolder.inKelpMaze = false;
        BGMManager.instance.SwitchBGMFade(6);
    }

    private void TeleportToEelCave()
    {
        GameDataHolder.inEelCave = true;
        GameDataHolder.inKelpMaze = false;
        BGMManager.instance.SwitchBGM(6);
    }

    public IEnumerator Fade(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.0f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        player.transform.localPosition = teleportPosition;
        yield return new WaitForSeconds(0.05f);
        CheckLocation();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
    }
}
