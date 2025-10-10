using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NewDoorPuzzle : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] public int doorHealth;
    [SerializeField] private GameObject electricity;
    private MeshRenderer meshRenderer;
    private Material[] originalMats;
    [SerializeField] private Material[] hitMaterials;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;
    //[SerializeField] private AudioClip doorBuzz;
    [SerializeField] private AudioClip unlockSound;

    private DoorScript2 doorController;
    private UItext textController;

    [SerializeField] private GameObject doorControl;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMats = meshRenderer.sharedMaterials;
        audioSource = GetComponent<AudioSource>();


    }

    private void Start()
    {
        //doorController = doorHinge.GetComponent<DoorScript2>();
        doorController = doorControl.GetComponent<DoorScript2>();
        textController = doorControl.GetComponent<UItext>();
    }

    private void Update()
    {
        if (doorHealth == 0)
        {
            doorController.canOpen = true;
            doorController.close = true;
            textController.Text = "Mechanism unjammed. Door unlocked.";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife" && isOn == true && (doorHealth > 0))
        {
            int randomNoise = Random.Range(0, 2);
            audioSource.PlayOneShot(hitSounds[randomNoise]);
            Debug.Log("doorHit");
            doorHealth -= 1;
            meshRenderer.sharedMaterials = hitMaterials;
            Invoke("SetOrigMaterial", 0.10f);

            if (doorHealth <= 0)
            {
                //audioSource.PlayOneShot(unlockSound);
                //ScreenShakeManager.instance.StartCameraShake(.5f, 1.5f);
                StartCoroutine(UnlockDoorSFX());
                electricity.SetActive(false);
                Debug.Log("door unjam");
                isOn = false;
            }
        }
    }

    private void SetOrigMaterial()
    {
        meshRenderer.sharedMaterials = originalMats;
    }

    IEnumerator UnlockDoorSFX()
    {
        yield return new WaitForSeconds(.7f);
        audioSource.PlayOneShot(unlockSound);
    }

    /*public GameObject interactionTriggerObj;
            public void SetPuzzleActive()
            {
                InteractorTrigger interactor = interactionTriggerObj.GetComponent<InteractorTrigger>();

                if (doorHealth <= 0)
                {
                    interactor.enabled = true;
                    Debug.Log("Interactor set active");
                }
            }*/
    }
