using UnityEngine;
using System.Collections;

public class BreathingManager : MonoBehaviour
{
    public static BreathingManager instance {get; private set;}
    public AudioClip[] breathSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Breathing Manager found in scene");
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = breathSound[0];
        audioSource.Play();
    }

    public void SwitchBreathRate(int clipNumber)
    {
        audioSource.clip = breathSound[clipNumber];
        audioSource.Play();
    }

    public void StopBreathe()
    {
        audioSource.Stop();
    }
}
