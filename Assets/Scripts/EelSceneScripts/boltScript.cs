using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boltScript : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int boltHealth;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip explosionSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Knife" && isOn == true && (boltHealth > 0))
        {
            int randomNoise = Random.Range(0,3);
            audioSource.PlayOneShot(hitSounds[randomNoise]);
            boltHealth -= 1;

            if(boltHealth <= 0)
            {
                audioSource.PlayOneShot(explosionSound);
                GameDataHolder.eelIsDead = true;
                isOn = false;
            }
        }
    }

    public void ResetBoltHealth()
    {
        boltHealth = 1;
    }
}
