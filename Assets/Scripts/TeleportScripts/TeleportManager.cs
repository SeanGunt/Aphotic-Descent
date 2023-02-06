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

    public void TelportToLab()
    {
        GameDataHolder.inLab = true;
        GameDataHolder.inKelpMaze = false;
        BGMManager.instance.SwitchBGM(2);
        fogCube.SetActive(false);
    }
}
