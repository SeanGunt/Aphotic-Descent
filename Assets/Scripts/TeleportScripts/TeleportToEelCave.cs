using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToEelCave : MonoBehaviour
{
    [SerializeField] private GameObject teleLocation;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void TeleportPlayer()
    {
        CrabLabAmbientNoiseManager.inLab = false;
        player.transform.localPosition = teleLocation.transform.position;
    }
}
