using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveAmbience : MonoBehaviour
{
    [SerializeField] private GameObject[] soundPoints;
    [SerializeField] private AudioClip[] caveAmbientSounds;
    private AudioSource audioSource;
    private float soundTimer;

    private void Awake()
    {
        soundTimer = Random.Range(2f,3f);
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0 && GameDataHolder.inPsShrimpCave)
        {
            int randomNoise = Random.Range(0,6);
            int randomPoint = Random.Range(0,4);
            AudioSource.PlayClipAtPoint(caveAmbientSounds[randomNoise], soundPoints[randomPoint].transform.position, audioSource.volume * 2f);
            soundTimer = Random.Range(2f, 3f);
        }
    }
}
