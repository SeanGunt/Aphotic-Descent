using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerCutscene : MonoBehaviour
{
    [SerializeField] private Animator anglerCutscene;
    [SerializeField] private AudioClip anglerRoar;
    [SerializeField] private AudioSource audioSource;
    private BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = this.GetComponent<BoxCollider>();
        anglerCutscene = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anglerCutscene.SetBool("activatedAngler", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anglerCutscene.SetBool("activatedAngler", true);
            audioSource.PlayOneShot(anglerRoar);
            boxCollider.enabled = false;
            Invoke("SetInactive", 6.5f);
        }
    }

    private void SetInactive()
    {
        this.gameObject.SetActive(false);
    }
}
