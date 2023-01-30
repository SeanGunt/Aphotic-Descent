using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardZone : MonoBehaviour
{
    private Animator animator;
    private BoxCollider bc;
    private AudioSource audioSource;
    [SerializeField] private AudioClip trapSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();
    }
    void OnTriggerStay(Collider other)
    {
        PlayerHealthController controller = other.GetComponent<PlayerHealthController>();

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

    public void Blacklighted()
    {
        audioSource.PlayOneShot(trapSound);
        animator.SetTrigger("steppedOn");
        bc.enabled = false;
    }
}
