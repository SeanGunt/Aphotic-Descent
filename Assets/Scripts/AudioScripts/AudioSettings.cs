using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private static readonly string BGMPref = "BGM Pref";
    private static readonly string SFXPref = "SFX Pref";
    private static readonly string MasPref = "Mas Pref";
    private static readonly string ambiencePref = "Ambience Pref";
    private float bgmFloat, sfxFloat, masFloat, ambienceFloat;
    public AudioSource bgmAudio;
    public AudioSource[] sfxAudio;
    public AudioSource[] ambience;

    void Awake()
    {
       ContinueSettings(); 
    }

    private void ContinueSettings()
    {
        bgmFloat = PlayerPrefs.GetFloat(BGMPref);
        sfxFloat = PlayerPrefs.GetFloat(SFXPref);
        masFloat = PlayerPrefs.GetFloat(MasPref);
        ambienceFloat = PlayerPrefs.GetFloat(ambiencePref);

        bgmAudio.volume = bgmFloat;

        AudioListener.volume = masFloat;

        for(int i = 0; i< sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxFloat;
        }
    }
}
