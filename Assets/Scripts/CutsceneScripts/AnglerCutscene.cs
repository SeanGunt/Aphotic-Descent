using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerCutscene : MonoBehaviour
{
    [SerializeField] private Animator anglerCutscene;
    [SerializeField] private AudioClip anglerRoar;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
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
            Destroy(this.gameObject, 6.5f);
        }
    }
}
