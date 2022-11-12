using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardZone : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        PlayerHealthController controller = other.GetComponent<PlayerHealthController>();

        if (controller != null)
        {
            controller.ChangeHealth(-8.5f);
            controller.TakeDamage();
            controller.isBleeding = true; 
        }
    }
    void OnTriggerEnter (Collider other)
    {
    if (other.CompareTag("Player"))
    {
        Destroy(this);
    }
    }
}
