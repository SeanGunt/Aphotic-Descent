using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip cageSound;
    [SerializeField] private GameObject ropeToCheck;
    private bool soundPlayed;
    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!soundPlayed && !ropeToCheck.activeInHierarchy)
        {
            audioSource.PlayOneShot(cageSound);
            soundPlayed = true;
        }
    }
}
