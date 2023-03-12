using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boltScript : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int boltHealth;
    //[SerializeField] private GameObject electricity;
    //private MeshRenderer meshRenderer;
    //private Material[] originalMats;
    //[SerializeField] private Material[] hitMaterials;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip explosionSound;

    private void Awake()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        //originalMats = meshRenderer.sharedMaterials;
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Knife" && isOn == true && (boltHealth > 0))
        {
            int randomNoise = Random.Range(0,3);
            audioSource.PlayOneShot(hitSounds[randomNoise]);
            Debug.Log("generatorHit");
            boltHealth -= 1;
            //meshRenderer.sharedMaterials = hitMaterials;
            //Invoke("SetOrigMaterial", 0.10f);

            if(boltHealth <= 0)
            {
                audioSource.PlayOneShot(explosionSound);
                StartCoroutine("StopGenSounds");
                //electricity.SetActive(false);
                Debug.Log("generator broke");
                isOn = false;
            }
        }
    }

    public void ResetBoltHealth()
    {
        boltHealth = 1;
    }

    private IEnumerator StopGenSounds()
    {
        yield return new WaitForSeconds(1.0f);
        audioSource.Stop();
    }
}
