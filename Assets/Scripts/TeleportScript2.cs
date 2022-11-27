using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript2 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.transform.localPosition = new Vector3(-105.3f, -44.78f, 241.13f);
        }
    }
}
