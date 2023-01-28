using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabLabAmbientNoiseManager : MonoBehaviour
{
    [SerializeField] private GameObject[] soundPoints;
    [SerializeField] private AudioClip[] crabLabAmbientSounds;
    private AudioSource audioSource;
    private float soundTimer;

    private void Awake()
    {
        soundTimer = Random.Range(5f,7f);
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0 && GameDataHolder.inLab)
        {
            int randomNoise = Random.Range(0,16);
            int randomPoint = Random.Range(0,6);
            AudioSource.PlayClipAtPoint(crabLabAmbientSounds[randomNoise], soundPoints[randomPoint].transform.position, audioSource.volume * 1.5f);
            soundTimer = Random.Range(5f, 7f);
        }
    }
}
