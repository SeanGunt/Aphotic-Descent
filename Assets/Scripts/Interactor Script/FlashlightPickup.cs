using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashlightPickup : MonoBehaviour
{
    public bool inTrigger;
    public GameObject tutTextObj;
    public TextMeshProUGUI tutText;
    [SerializeField] private ClearUIText clearUIText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        if (GameDataHolder.flashlightHasBeenPickedUp)
        {
            this.gameObject.SetActive(false);
        }
    }
 
    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
 
    void Update()
    {
        if (inTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                audioSource.PlayOneShot(pickupSound);
                clearUIText.CancelInvoke();
                clearUIText.Invoke("ClearUI", 2);
                tutTextObj.SetActive(true);
                tutText.text = "Flashlight Acquired";
                GameDataHolder.flashlightHasBeenPickedUp = true;
                DataPersistenceManager.instance.SaveGame();
                Destroy(this.gameObject);
            }
        }
    }
}
