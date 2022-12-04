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
            bGMManager.state = BGMManager.State.StopMusic;
            GameDataHolder.musicStopped = true;
            player.transform.localPosition = new Vector3(-105.3f, -44.78f, 241.13f);
            DataPersistenceManager.instance.SaveGame();
        }
    }
}
