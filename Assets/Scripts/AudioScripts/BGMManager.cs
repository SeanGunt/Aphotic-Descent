using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private ffScr freakFishScript;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] AudioClip[] bgms;
    [HideInInspector] public float audioVolume;
    private float mixerBGMVolume = 0f;

    private void Awake()
    {
        audioMixer.SetFloat("BGM", mixerBGMVolume);
        audioSource = this.GetComponent<AudioSource>();
        SwitchBGM(0);
    }

    private void Start()
    {
        if (GameDataHolder.musicStopped)
        {
            SwitchBGM(4);
        }
    }

    public void SwitchBGM(int clipNumber)
    {
        audioSource.clip = bgms[clipNumber];
        audioSource.Play();
    }

    public void SwitchBGMFade(int clipNumber)
    {
        StartCoroutine("Fade", clipNumber);
    }   

    public IEnumerator Fade(int clipNumber)
    {
        float waitTime = 30f;
        float startVolume = 0f;
        float endVolume = -25f;

        while(endVolume < startVolume)
        {
            startVolume -= Time.deltaTime * waitTime;
            audioMixer.SetFloat("BGM", startVolume);
            yield return null;
        }
        audioMixer.SetFloat("BGM", mixerBGMVolume);
        audioSource.clip = bgms[clipNumber];
        audioSource.Play();
    }

    private void StopMusic()
    {
        audioSource.Stop();
    }
}
