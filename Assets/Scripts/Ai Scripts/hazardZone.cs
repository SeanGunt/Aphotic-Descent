using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardZone : MonoBehaviour
{
    private Animator animator;
    private BoxCollider bc;
    private AudioSource audioSource;
    [SerializeField] private AudioClip trapSound;
    void OnTriggerStay(Collider other)
    {
        PlayerHealthController controller = other.GetComponent<PlayerHealthController>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();

        if (controller != null)
        {
            audioSource.PlayOneShot(trapSound);
            controller.ChangeHealth(-8.5f);
            controller.TakeDamage();
            controller.isBleeding = true; 
            animator.SetTrigger("steppedOn");
            bc.enabled = false;
        }
    }
}
