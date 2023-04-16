using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFCutsceneTriggers : MonoBehaviour
{
    [SerializeField] private AudioSource ffAudioSource, pistolShrimpGunAudioSource;
    [SerializeField] private AudioClip gunShot, ffSnarl, ffDie;

    public void PlaySnarl()
    {
        ffAudioSource.PlayOneShot(ffDie);
    }

    public void PlayGunshot()
    {
        pistolShrimpGunAudioSource.PlayOneShot(gunShot);
    }

    public void PlayDie()
    {
        ffAudioSource.PlayOneShot(ffSnarl);
    }
}
