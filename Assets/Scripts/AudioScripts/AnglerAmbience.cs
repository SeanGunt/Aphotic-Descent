using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerAmbience : MonoBehaviour
{
    [SerializeField] private GameObject[] soundPoints;
    [SerializeField] private AudioClip[] audioClips;
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
        if (soundTimer <= 0 && GameDataHolder.inAnglerTrench)
        {
            int randomNoise = Random.Range(0, audioClips.Length);
            int randomPoint = Random.Range(0, soundPoints.Length);
            AudioSource.PlayClipAtPoint(audioClips[randomNoise], soundPoints[randomPoint].transform.position, audioSource.volume * 3.0f);
            soundTimer = Random.Range(5f, 7f);
        }
    }
}
