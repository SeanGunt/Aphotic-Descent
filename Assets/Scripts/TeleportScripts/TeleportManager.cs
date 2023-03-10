using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportManager : MonoBehaviour
{
    public UnityEvent unityEvent;
    private GameObject player, fogCube;
    [SerializeField] private Vector3 teleportPosition;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fogCube = GameObject.FindGameObjectWithTag("FogCube");
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
        BGMManager.instance.SwitchBGM(0);
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
