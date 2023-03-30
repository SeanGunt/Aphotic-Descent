using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRumble : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] rumbleSounds;
    [SerializeField] private int index;
    private bool canTrigger = true;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && canTrigger)
        {
            ScreenShakeManager.instance.StartCameraShake(2f, 2f);
            audioSource.PlayOneShot(rumbleSounds[index]);
            canTrigger = false;
        }
    }
}
