using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] AudioClip[] bgms;
    [HideInInspector] public float audioVolume;
    private float mixerBGMVolume = 0f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one instance of BGMManager in scene");
        }
        instance = this;
        audioMixer.SetFloat("BGM", mixerBGMVolume);
        audioMixer.SetFloat("Cutoff freq", 22000f);
        audioMixer.SetFloat("Resonance", 1.0f);
        audioMixer.SetFloat("BGM", 0f);
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(GameDataHolder.inSub)
        {
            SwitchBGM(2);
        }
        if(GameDataHolder.inKelpMaze)
        {
            SwitchBGM(0);
        }
        if (GameDataHolder.inLab)
        {
            SwitchBGM(5);
        }
        if(GameDataHolder.inPsShrimpCave)
        {
            SwitchBGM(7);
        }
        if(GameDataHolder.inMudMarsh)
        {
            SwitchBGM(7);
        }
        if(GameDataHolder.inAnglerTrench)
        {
            SwitchBGM(9);
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

    public void Pause()
    {
        audioMixer.SetFloat("Cutoff freq", 470f);
        audioMixer.SetFloat("Resonance", 1.3f);
        audioMixer.SetFloat("BGM", -10f);
    }

    public void EndPause()
    {
        audioMixer.SetFloat("Cutoff freq", 22000f);
        audioMixer.SetFloat("Resonance", 1.0f);
        audioMixer.SetFloat("BGM", 0f);
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

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
