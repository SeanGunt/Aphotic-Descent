using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sandSounds;
    [SerializeField] private AudioClip[] metalSounds;
    [SerializeField] private AudioClip[] rockSands;
    private LayerMask walkingLayer;
    private GameObject player;
    private PlayerMovement pm;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pm = player.GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        walkingLayer = collision.collider.gameObject.layer;
    }

    public void PlayFootStepSounds()
    {
        if (pm.isGrounded)
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

            if (walkingLayer == 16)
            {
                int randomNoise = Random.Range(0,3);
                audioSource.PlayOneShot(rockSands[randomNoise]);
            }
        }
    }
}
