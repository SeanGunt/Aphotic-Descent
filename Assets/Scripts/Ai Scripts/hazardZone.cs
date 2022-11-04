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
            controller.ChangeHealth(-1.5f);
            controller.TakeDamage();
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
