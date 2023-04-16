using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviour
{
    private GameObject player, fogCube;
    private UItext uItext;
    [SerializeField] private Vector3 teleportPosition;
    private PlayerMovement playerMovement;
    private GameObject mainCamera;
    [SerializeField] private Image fadeToBlackImage;
    [SerializeField] private int teleportNumber;
    private bool activated;

    private void Awake()
    {
        activated = false;
        player = GameObject.FindGameObjectWithTag("Player");
        fogCube = GameObject.FindGameObjectWithTag("FogCube");
        playerMovement = player.GetComponent<PlayerMovement>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        uItext = this.GetComponent<UItext>();
    }

    public void BasicTeleport()
    {
        if (!activated)
        {
            uItext.enabled = false;
            StartCoroutine(Fade(1f));
            activated = true;
        }
    }

    public void LabTeleport()
    {
        if (!activated)
        {
            uItext.enabled = false;
            StartCoroutine(FFFade(1f));
            activated = true;
        }
    }

    public void RidgeTeleport()
    {
        if(!activated)
        {
            uItext.enabled = false;
            StartCoroutine(SMLFade(1f));
            activated = true;
        }
    }

    public void MarshTeleport()
    {
        if(!activated)
        {
            uItext.enabled = false;
            StartCoroutine(MarshFade(1f));
            activated = true;
        }
    }

    private void CheckLocation()
    {
        uItext.enabled = true;
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

        if (teleportNumber == 5)
        {
            TeleportToMudMarsh();
        }
        if(teleportNumber == 6)
        {
            TeleportToTrench();
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

    private void TeleportToMudMarsh()
    {
        GameDataHolder.inPsShrimpCave = false;
        GameDataHolder.inMudMarsh = true;
        GameDataHolder.pistolshrimpFound = true;
        BGMManager.instance.SwitchBGMFade(7);
    }

    private void TeleportToTrench()
    {
        GameDataHolder.inMudMarsh = false;
        GameDataHolder.inAnglerTrench = true;
        GameDataHolder.shrimpmanFound = true;
        BGMManager.instance.SwitchBGMFade(9);
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
        yield return new WaitForSeconds(0.3f);
        CheckLocation();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
    }

    public IEnumerator FFFade(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.0f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        FFCutscene.instance.StartCutscene();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        yield return new WaitForSeconds(6f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        FFCutscene.instance.EndCutscene();
        player.transform.localPosition = teleportPosition;
        yield return new WaitForSeconds(0.3f);
        CheckLocation();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
    }

    public IEnumerator SMLFade(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.0f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        SMLCutscene.instance.StartCutscene();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        SMLCutscene.instance.PlaySound(1);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        yield return new WaitForSeconds(2.3f);
        SMLCutscene.instance.PlaySound(2);
        yield return new WaitForSeconds(0.2f);
        SMLCutscene.instance.PlaySound(3);
        yield return new WaitForSeconds(1.1f);
        SMLCutscene.instance.EndCutscene();
        player.transform.localPosition = teleportPosition;
        yield return new WaitForSeconds(0.3f);
        CheckLocation();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
    }

    public IEnumerator MarshFade(float t)
    {
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0.0f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        MudMarshCutscene.instance.StartCutscene();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        while(fadeToBlackImage.color.a < 1.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a + Time.deltaTime / t);
            yield return null;
        }
        player.transform.position = teleportPosition;
        yield return new WaitForSeconds(0.3f);
        CheckLocation();
        MudMarshCutscene.instance.EndCutscene();
        while(fadeToBlackImage.color.a >= 0.0f)
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, fadeToBlackImage.color.a - Time.deltaTime / t);
            yield return null;
        }
        
    }
}
