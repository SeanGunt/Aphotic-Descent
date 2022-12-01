using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sandSounds;
    [SerializeField] private AudioClip[] metalSounds;
    private LayerMask walkingLayer;
    private void OnCollisionEnter(Collision collision)
    {
        walkingLayer = collision.collider.gameObject.layer;
    }

    public void PlayFootStepSounds()
    {
        if (walkingLayer == 3)
        {
            int randomNoise = Random.Range(0, 3);
            audioSource.PlayOneShot(sandSounds[randomNoise]);
        }
        if (walkingLayer == 0)
        {
            int randomNoise = Random.Range(0, 4);
            audioSource.PlayOneShot(metalSounds[randomNoise]);
        }
    }
}
