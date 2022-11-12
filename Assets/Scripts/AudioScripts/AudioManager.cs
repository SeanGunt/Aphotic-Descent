using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string Firstplay = "First Play";
    private static readonly string BGMPref = "BGM Pref";
    private static readonly string SFXPref = "SFX Pref";
    private static readonly string MasPref = "Mas Pref";
    private static readonly string AmbiencePref ="Ambience Pref";
    private int firstPlayInt;
    public Slider bgmSlider, sfxSlider, masSlider, ambienceSlider; 
    private float bgmFloat, sfxFloat, masFloat, ambienceFloat;
    public AudioSource bgmAudio;
    public AudioSource[] sfxAudio;
    public AudioSource[] ambience;

   // public AudioListener masAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(Firstplay);

        if(firstPlayInt == 0)
        {
            bgmFloat = .50f;
            sfxFloat = .50f;
            masFloat = .50f;
            ambienceFloat = .50f;

            bgmSlider.value = bgmFloat;
            sfxSlider.value = sfxFloat;
            masSlider.value = masFloat;
            ambienceSlider.value = ambienceFloat;

            PlayerPrefs.SetFloat(BGMPref, bgmFloat);
            PlayerPrefs.SetFloat(SFXPref, sfxFloat);
            PlayerPrefs.SetFloat(MasPref, masFloat);
            PlayerPrefs.SetFloat(AmbiencePref, ambienceFloat);
            PlayerPrefs.SetInt(Firstplay, -1);
        }
        else
        {
            bgmFloat = PlayerPrefs.GetFloat(BGMPref);
            bgmSlider.value = bgmFloat;
            sfxFloat = PlayerPrefs.GetFloat(SFXPref);
            sfxSlider.value = sfxFloat;
            masFloat = PlayerPrefs.GetFloat(MasPref);
            masSlider.value = masFloat;
            ambienceFloat = PlayerPrefs.GetFloat(AmbiencePref);
            ambienceSlider.value = ambienceFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BGMPref, bgmSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
        PlayerPrefs.SetFloat(MasPref, masSlider.value);
        PlayerPrefs.SetFloat(AmbiencePref, ambienceSlider.value);
        PlayerPrefs.Save();
    }

    void OnApplicationFocus(bool inFocus)
    {
        if(!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        bgmAudio.volume = bgmSlider.value;

        for(int i = 0; i< sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxSlider.value;
        }

        AudioListener.volume = masSlider.value;
    }
}
