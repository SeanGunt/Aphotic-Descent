using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabLabAmbientNoiseManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject[] soundPoints;
    [SerializeField] private AudioClip[] crabLabAmbientSounds;
    private AudioSource audioSource;
    private float soundTimer;
    public static bool inLab;

    public void LoadData(GameData data)
    {
        inLab = data.inLab;
    }

    public void SaveData(GameData data)
    {
        data.inLab = inLab;
    }

    private void Awake()
    {
        inLab = false;
        soundTimer = Random.Range(5f,7f);
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0 && inLab)
        {
            int randomNoise = Random.Range(0,16);
            int randomPoint = Random.Range(0,6);
            AudioSource.PlayClipAtPoint(crabLabAmbientSounds[randomNoise], soundPoints[randomPoint].transform.position, audioSource.volume * 1.5f);
            soundTimer = Random.Range(5f, 7f);
        }
    }
}
