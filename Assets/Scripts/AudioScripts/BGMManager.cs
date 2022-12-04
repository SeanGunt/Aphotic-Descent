using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private ffScr freakFishScript;
    [SerializeField] AudioClip[] bgms;
    [HideInInspector] public State state;
    [HideInInspector] public float audioVolume;

    public enum State
    {
        normalBGM, ffChaseMusic, transition, StopMusic
    }

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = bgms[0];
    }

    private void Start()
    {
        if (GameDataHolder.musicStopped)
        {
            state = State.StopMusic;
        }
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.transition:

            break;

            case State.StopMusic:
                    StopMusic();
            break;
            case State.normalBGM:
                    PlayNormalBGM();
            break;

            case State.ffChaseMusic:
                    PlayFFChaseSound();
            break;
        }
    }

    public void PlayNormalBGM()
    {
        audioSource.clip = bgms[0];
        audioSource.Play();
        state = State.transition;
    }

    private void PlayFFChaseSound()
    {
        audioSource.clip = bgms[1];
        audioSource.Play();
        state =  State.transition;
    }

    private void StopMusic()
    {
        audioSource.Stop();
        state = State.transition;
    }
}
