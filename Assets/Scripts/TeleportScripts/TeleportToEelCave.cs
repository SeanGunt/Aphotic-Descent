using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToEelCave : MonoBehaviour
{
    [SerializeField] private GameObject teleLocation;
    private GameObject player;
    public void TeleportPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameDataHolder.inLab = false;
        player.transform.localPosition = teleLocation.transform.position;
    }
}
