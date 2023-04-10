using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boltScript : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int boltHealth;
    [SerializeField] private GameObject electricity;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip explosionSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(GameDataHolder.eelIsDead)
        {
            electricity.SetActive(false);
            boltHealth = 0;
            audioSource.Stop();
        }
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
                electricity.SetActive(false);
                audioSource.Stop();
                GameDataHolder.eelIsDead = true;
                GameDataHolder.eelFound = true;
                isOn = false;
            }
        }
    }

    public void ResetBoltHealth()
    {
        boltHealth = 1;
    }
}
