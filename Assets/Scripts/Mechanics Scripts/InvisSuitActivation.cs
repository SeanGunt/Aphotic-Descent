using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvisSuitActivation : MonoBehaviour
{   
    private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private TextMeshProUGUI tutText;
    public GameObject tutTextObj;
    [SerializeField] private ClearUIText clearUIText;
    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        if (GameDataHolder.invisibilityAcquired)
        {
            this.gameObject.SetActive(false);
            Debug.Log("Your mom");
        }
    }
    void DestroyMyself()
    {
        Destroy(this.gameObject);
    }

    public void UpgradeSuit(InvisibilityMechanic controller)
    {
        GameDataHolder.invisibilityAcquired = true;
        clearUIText.CancelInvoke();
        clearUIText.Invoke("ClearUI", 4);
        tutTextObj.SetActive(true);
        tutText.text = "Invisibility Gadget Acquired, Q To Go Invisible";
        audioSource.PlayOneShot(pickupSound);
        controller.SetInvisUIActive();
        DestroyMyself();
    }
}
