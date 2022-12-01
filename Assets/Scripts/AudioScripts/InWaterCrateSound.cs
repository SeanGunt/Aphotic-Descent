using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWaterCrateSound : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] inWaterSounds;
    private void Awake()
    {
        int randomNoise = Random.Range(0, 3);
        audioSource.PlayOneShot(inWaterSounds[randomNoise]);
    }
}
