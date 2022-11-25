using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardZone : MonoBehaviour
{
    private Animator animator;
    private BoxCollider bc;
    void OnTriggerStay(Collider other)
    {
        PlayerHealthController controller = other.GetComponent<PlayerHealthController>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();

        if (controller != null)
        {
            controller.ChangeHealth(-8.5f);
            controller.TakeDamage();
            controller.isBleeding = true; 
            animator.SetTrigger("steppedOn");
            bc.enabled = false;
        }
    }
}
