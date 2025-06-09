using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorPuzzle : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int genHealth;
    [SerializeField] private GameObject electricity;
    private MeshRenderer meshRenderer;
    private Material[] originalMats;
    [SerializeField] private Material[] hitMaterials;
    //private AudioSource audioSource;
    //[SerializeField] private AudioClip[] hitSounds;
    //[SerializeField] private AudioClip explosionSound;
    
    private DoorScript2 doorController;
    private UItext textController;

    [SerializeField] private GameObject doorControl;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMats = meshRenderer.sharedMaterials;
        //audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        //doorController = doorHinge.GetComponent<DoorScript2>();
        doorController = doorControl.GetComponent<DoorScript2>();
        textController = doorControl.GetComponent<UItext>();
    }

    private void Update()
    {
        if (genHealth == 0)
        {
            doorController.canOpen = true;
            doorController.close = true;
            textController.Text = "Mechanism unjammed. Door unlocked.";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Knife" && isOn == true && (genHealth > 0))
        {
            //int randomNoise = Random.Range(0,3);
            //audioSource.PlayOneShot(hitSounds[randomNoise]);
            Debug.Log("generatorHit");
            genHealth -= 1;
            meshRenderer.sharedMaterials = hitMaterials;
            Invoke("SetOrigMaterial", 0.10f);

            if(genHealth <= 0)
            {
                //audioSource.PlayOneShot(explosionSound);
                ScreenShakeManager.instance.StartCameraShake(.5f, 1.5f);
                //StartCoroutine("StopGenSounds");
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
