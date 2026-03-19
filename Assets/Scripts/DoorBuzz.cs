using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorBuzz : MonoBehaviour
{
    private AudioSource audioSource;

    public NewDoorPuzzle newDoorPuzzle;
    private int dHealth;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //audioSource.PlayOneShot(doorBuzz);
    }

    // Update is called once per frame
    void Update()
    {
        GetHealth();
        
        if (newDoorPuzzle.doorHealth <= 0)
        {
            StopAudio();
        }
    }

    public void GetHealth()
    {
        dHealth = newDoorPuzzle.doorHealth;
    }
    
    public void StopAudio()
    {
        audioSource.Stop();
    }
    
}
