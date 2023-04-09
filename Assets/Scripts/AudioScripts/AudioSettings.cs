using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private static readonly string BGMPref = "BGM Pref";
    private static readonly string SFXPref = "SFX Pref";
    private static readonly string MasPref = "Mas Pref";
    private float bgmFloat, sfxFloat, masFloat;
    public AudioSource bgmAudio;
    public AudioSource[] sfxAudio;

    void Awake()
    {
       ContinueSettings(); 
    }

    private void ContinueSettings()
    {
        bgmFloat = PlayerPrefs.GetFloat(BGMPref);
        sfxFloat = PlayerPrefs.GetFloat(SFXPref);
        masFloat = PlayerPrefs.GetFloat(MasPref);

        bgmAudio.volume = bgmFloat;

        AudioListener.volume = masFloat;

        for(int i = 0; i< sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxFloat;
        }
    }
}
