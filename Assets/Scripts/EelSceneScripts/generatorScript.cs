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
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip explosionSound;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMats = meshRenderer.sharedMaterials;
        audioSource = GetComponent<AudioSource>();
        
        if(GameDataHolder.eelIsDead)
        {
            electricity.SetActive(false);
            genHealth = 0;
            audioSource.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Knife" && isOn == true && (genHealth > 0))
        {
            int randomNoise = Random.Range(0,3);
            audioSource.PlayOneShot(hitSounds[randomNoise]);
            Debug.Log("generatorHit");
            genHealth -= 1;
            meshRenderer.sharedMaterials = hitMaterials;
            Invoke("SetOrigMaterial", 0.10f);

            if(genHealth <= 0)
            {
                audioSource.PlayOneShot(explosionSound);
                ScreenShakeManager.instance.StartCameraShake(1f, 3f);
                StartCoroutine("StopGenSounds");
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

    private IEnumerator StopGenSounds()
    {
        yield return new WaitForSeconds(1.0f);
        audioSource.Stop();
    }
}
