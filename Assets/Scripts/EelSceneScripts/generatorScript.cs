using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatorScript : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int genHealth;
    [SerializeField] private GameObject electricity;
    private MeshRenderer meshRenderer;
    private Material[] originalMats;
    [SerializeField] private Material[] hitMaterials;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip explosionSound;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMats = meshRenderer.sharedMaterials;
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Knife" && isOn == true && (genHealth > 0))
        {
            audioSource.PlayOneShot(hitSound);
            Debug.Log("generatorHit");
            genHealth -= 1;
            meshRenderer.sharedMaterials = hitMaterials;
            Invoke("SetOrigMaterial", 0.10f);

            if(genHealth <= 0)
            {
                audioSource.PlayOneShot(explosionSound);
                electricity.SetActive(false);
                Debug.Log("generator broke");
                isOn = false;
            }
        }
    }

    private void SetOrigMaterial()
    {
        meshRenderer.sharedMaterials = originalMats;
    }
}
