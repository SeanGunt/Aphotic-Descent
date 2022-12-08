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
        normalBGM, ffChaseMusic, transition, StopMusic, CrabLabBGM, EelIdle, EelChase
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
            state = State.CrabLabBGM;
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

            case State.CrabLabBGM:
                    PlayCrabLabBGM();
            break;

            case State.EelIdle:
                    PlayEelIdle();
            break;

            case State.EelChase:
                    PlayEelChase();
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

    private void PlayCrabLabBGM()
    {
        audioSource.clip = bgms[2];
        audioSource.Play();
        state = State.transition;
    }

    private void PlayEelIdle()
    {
        audioSource.clip = bgms[3];
        audioSource.Play();
        state = State.transition;
    }

    private void PlayEelChase()
    {
        audioSource.clip = bgms[4];
        audioSource.Play();
        state = State.transition;
    }

    private void StopMusic()
    {
        audioSource.Stop();
        state = State.transition;
    }
}
