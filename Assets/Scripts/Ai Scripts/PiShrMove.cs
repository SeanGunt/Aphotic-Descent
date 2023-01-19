using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiShrMove : MonoBehaviour
{
  public GameObject PistolMan;

    [SerializeField] private Animator PiShrAnimatorControl;

    public AudioSource PiShrSound;
    public AudioClip GarbledRadio;

    public void Awake() 
    {
        PiShrSound = GetComponent<AudioSource>();
        PiShrSound.Play(0);
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag ("Player"))
        {
            PiShrAnimatorControl.SetBool("MoveAway", true);
        }
    }

}
