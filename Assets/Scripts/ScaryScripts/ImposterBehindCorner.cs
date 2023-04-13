using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterBehindCorner : MonoBehaviour
{
    [SerializeField] private GameObject CrabLabImposter;
    [SerializeField] private Animator imposterAnimator;
    [SerializeField] private AudioClip scaryStinger;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        imposterAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            imposterAnimator.SetBool("isSeen", true);
            audioSource.PlayOneShot(scaryStinger);
            Invoke("SetInactive", 1.5f);
        }
    }

    private void SetInactive()
    {
        this.gameObject.SetActive(false);
    }
}
