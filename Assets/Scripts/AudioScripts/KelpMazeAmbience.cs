using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpMazeAmbience : MonoBehaviour
{
    [SerializeField] private GameObject[] soundPoints;
    [SerializeField] private AudioClip[] kelpMazeAmbientSounds;
    private AudioSource audioSource;
    private float soundTimer;

    private void Awake()
    {
        soundTimer = Random.Range(1f,4f);
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0 && GameDataHolder.inKelpMaze)
        {
            int randomNoise = Random.Range(0,3);
            int randomPoint = Random.Range(0,9);
            AudioSource.PlayClipAtPoint(kelpMazeAmbientSounds[randomNoise], soundPoints[randomPoint].transform.position, audioSource.volume * 1.5f);
            soundTimer = Random.Range(1f, 4f);
        }
    }
}
