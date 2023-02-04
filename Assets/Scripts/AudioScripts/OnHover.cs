using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource = GetComponentInParent<AudioSource>();
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
