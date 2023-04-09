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
    private int firstPlayInt;
    public Slider bgmSlider, sfxSlider, masSlider;
    private float bgmFloat, sfxFloat, masFloat;
    public AudioSource bgmAudio;
    public AudioSource[] sfxAudio;

   // public AudioListener masAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(Firstplay);

        if(firstPlayInt == 0)
        {
            bgmFloat = .50f;
            sfxFloat = .50f;
            masFloat = .50f;

            bgmSlider.value = bgmFloat;
            sfxSlider.value = sfxFloat;
            masSlider.value = masFloat;

            PlayerPrefs.SetFloat(BGMPref, bgmFloat);
            PlayerPrefs.SetFloat(SFXPref, sfxFloat);
            PlayerPrefs.SetFloat(MasPref, masFloat);
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
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BGMPref, bgmSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
        PlayerPrefs.SetFloat(MasPref, masSlider.value);
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
