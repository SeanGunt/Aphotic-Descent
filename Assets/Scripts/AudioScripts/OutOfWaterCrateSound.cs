using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfWaterCrateSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] outOfWaterSounds;
    private void Awake()
    {
        int randomNoise = Random.Range(0, 3);
        audioSource.PlayOneShot(outOfWaterSounds[randomNoise]);
    }
}
