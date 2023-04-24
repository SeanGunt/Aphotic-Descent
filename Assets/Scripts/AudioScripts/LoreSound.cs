using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceLines;
    [SerializeField] private AudioClip loreSound;
    public void PlayLoreSound()
    {
        audioSource.PlayOneShot(loreSound);
    }

    public void PlayVoiceLines(int index)
    {
        audioSource.clip = voiceLines[index];
        audioSource.Play();
    }
}
