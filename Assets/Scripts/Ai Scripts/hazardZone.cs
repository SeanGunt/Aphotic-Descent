using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardZone : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        PlayerMovement controller = other.GetComponent<PlayerMovement>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
