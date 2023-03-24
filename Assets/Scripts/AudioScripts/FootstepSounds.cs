using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sandSounds;
    [SerializeField] private AudioClip[] metalSounds;
    [SerializeField] private AudioClip[] rockSounds;
    [SerializeField] private AudioClip[] mudSounds;
    [SerializeField] private LayerMask ignoreLayer;
    private LayerMask walkingLayer;
    private GameObject player;
    private PlayerMovement pm;
    private bool canPlaySound = true;
    private float soundTimer = 0.8f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pm = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position + Vector3.up, Vector3.down, out hit , 1.25f, ~ignoreLayer))
        {
            walkingLayer = hit.collider.gameObject.layer;
        }
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0)
        {
            canPlaySound = true;
        }
        else
        {
            canPlaySound = false;
        }
    }

    public void PlayFootStepSounds()
    {
        if (pm.isGrounded)
        {
            if (walkingLayer == 3 && canPlaySound)
            {
                int randomNoise = Random.Range(0,3);
                audioSource.PlayOneShot(sandSounds[randomNoise]);
                HandleTimer();
            }
            if (walkingLayer == 18 && canPlaySound)
            {
                int randomNoise = Random.Range(0,4);
                audioSource.PlayOneShot(metalSounds[randomNoise]);
                HandleTimer();
            }

            if (walkingLayer == 16 && canPlaySound)
            {
                int randomNoise = Random.Range(0,3);
                audioSource.PlayOneShot(rockSounds[randomNoise]);
                HandleTimer();
            }

            if (walkingLayer == 22 && canPlaySound)
            {
                int randomNoise = Random.Range(0,3);
                audioSource.PlayOneShot(mudSounds[randomNoise]);
                HandleTimer();
            }
        }
    }

    private void HandleTimer()
    {
        soundTimer = 0.8f;
    }
}
