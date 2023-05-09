using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boltScript : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int boltHealth;
    [SerializeField] private GameObject electricity;
    private AudioSource audioSource;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip explosionSound;
    private Material[] originalMaterials;
    [SerializeField] private Material[] hitMaterials;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        originalMaterials = skinnedMeshRenderer.sharedMaterials;

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
            skinnedMeshRenderer.sharedMaterials = hitMaterials;
            StartCoroutine(ChangeBackToOriginalMaterials());

            if(boltHealth <= 0)
            {
                audioSource.PlayOneShot(explosionSound);
                electricity.SetActive(false);
                GameDataHolder.eelIsDead = true;
                GameDataHolder.eelFound = true;
                isOn = false;
            }
        }
    }

    private IEnumerator ChangeBackToOriginalMaterials()
    {
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.sharedMaterials = originalMaterials;
    }
}
