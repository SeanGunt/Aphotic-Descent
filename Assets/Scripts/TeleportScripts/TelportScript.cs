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
            player.transform.localPosition = new Vector3(-60f, -30f, -70f);
        }
    }
}
