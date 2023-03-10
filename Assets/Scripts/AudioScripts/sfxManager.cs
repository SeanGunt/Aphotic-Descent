using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class sfxManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioMixer.SetFloat("SFX", 1f);
    }
}
