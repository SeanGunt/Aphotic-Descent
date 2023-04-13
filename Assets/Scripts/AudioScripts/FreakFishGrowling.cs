using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakFishGrowling : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] growlingSounds;
    [SerializeField] private AudioClip playerScream;
    private float randomNoiseTimer;
    public static bool hitPlayer;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        randomNoiseTimer = Random.Range(7,15);
        hitPlayer = false;
    }

    private void Update()
    {
        randomNoiseTimer -= Time.deltaTime;
        if (randomNoiseTimer <= 0 && !hitPlayer)
        {
            PlayGrowlingSound();
        }
    }

    private void PlayGrowlingSound()
    {
        int randomNoise =  Random.Range(0, 4);
        audioSource.PlayOneShot(growlingSounds[randomNoise]);
        randomNoiseTimer = Random.Range(7,15);
    }

    public void PlayScream()
    {
        playerAudioSource.PlayOneShot(playerScream);
    }
}
