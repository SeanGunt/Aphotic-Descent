using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript2 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private BGMManager bGMManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameDataHolder.musicStopped = true;
            CrabLabAmbientNoiseManager.inLab = true;
            bGMManager.SwitchBGM(2);
            player.transform.localPosition = new Vector3(-105.3f, -46.78f, 241.13f);
        }
    }
}
