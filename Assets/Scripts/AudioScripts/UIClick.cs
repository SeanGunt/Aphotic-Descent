using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClick : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
