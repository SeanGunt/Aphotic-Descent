using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissCageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            playerMovement.inPissCage = true;
        }
    }
}
