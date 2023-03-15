using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportManager : MonoBehaviour
{
    public UnityEvent unityEvent;
    private GameObject player, fogCube;
    [SerializeField] private Vector3 teleportPosition;
    private PlayerMovement playerMovement;
    private GameObject mainCamera;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fogCube = GameObject.FindGameObjectWithTag("FogCube");
        playerMovement = player.GetComponent<PlayerMovement>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            unityEvent.Invoke();
        }
    }

    public void BasicTeleport()
    {
        player.transform.localPosition = teleportPosition;
    }

    public void TeleportToKelpMaze()
    {
        GameDataHolder.inSub = false;
        GameDataHolder.inKelpMaze = true;
        GameDataHolder.inLab = false;
        GameDataHolder.inEelCave = false;
        playerMovement.inCutscene = true;
        mainCamera.SetActive(false);
        BGMManager.instance.StopMusic();
    }

    public void TeleportToRidge()
    {
        GameDataHolder.inLab = false;
        GameDataHolder.inKelpMaze = true;
        BGMManager.instance.SwitchBGMFade(0);
    }

    public void TelpeortToLab()
    {
        GameDataHolder.inLab = true;
        GameDataHolder.inKelpMaze = false;
        GameDataHolder.inEelCave = false;
        BGMManager.instance.SwitchBGM(5);
    }

    public void TeleportToEelCave()
    {
        BGMManager.instance.SwitchBGM(6);
        GameDataHolder.inLab = false;
        GameDataHolder.inKelpMaze = false;
        GameDataHolder.inEelCave = true;
    }
}
