using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerSounds : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip aoeSound, stunnedSound;

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
