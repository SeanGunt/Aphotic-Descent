using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrankAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip lungeSound, deathSound, biteSound, playerScream;
    
    private void Awake()
    {
        audioSource = this.GetComponentInParent<AudioSource>();
    }

    public void PlayLungeSound()
    {
        audioSource.PlayOneShot(lungeSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }

    public void PlayBiteSound()
    {
        audioSource.PlayOneShot(biteSound);
    }

    public void PlayScream()
    {
        audioSource.PlayOneShot(playerScream);
    }
}
