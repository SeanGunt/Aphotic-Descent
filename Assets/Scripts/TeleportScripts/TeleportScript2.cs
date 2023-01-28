using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript2 : MonoBehaviour
{
    private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            GameDataHolder.inLab = true;
            BGMManager.instance.SwitchBGM(2);
            player.transform.localPosition = new Vector3(-105.3f, -46.78f, 241.13f);
        }
    }
}
