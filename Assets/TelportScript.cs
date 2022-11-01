using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelportScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.transform.localPosition = new Vector3(-58.17f, -24.9f, -58.4f);
        }
    }
}
