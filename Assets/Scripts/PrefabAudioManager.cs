using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabAudioManager : MonoBehaviour
{
    private AudioSource thisAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    private void Awake()
    {
        thisAudioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        thisAudioSource.volume = sfxAudioSource.volume;
    }
}
