using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClick : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private Menus menuController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        menuController.ResetMenus();
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
