using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSound : MonoBehaviour
{
    public AudioClip knifeSound;
    public AudioSource audioSource;
    // Start is called before the first frame update
    public void PlayKnifeSound()
    {
        audioSource.PlayOneShot(knifeSound);
    }
}
