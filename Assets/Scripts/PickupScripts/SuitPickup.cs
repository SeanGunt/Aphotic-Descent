using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuitPickup : MonoBehaviour
{
    public GameObject tutTextObj;
    public TextMeshProUGUI tutText;
    [SerializeField] private ClearUIText clearUIText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    public GameObject DivingSuitSprite;

    public GameObject EelSuitSprite;

    private void Start()
    {
        if (GameDataHolder.hasUpgradedSuit)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void PickupSuit()
    {
        GameDataHolder.hasUpgradedSuit = true;
        tutTextObj.SetActive(true);
        DivingSuitSprite.SetActive(false);
        EelSuitSprite.SetActive(true);
        tutText.text = "Upgraded Suit Acquired, Hold Space To Ascend and Shift To Descend While In Water";
        clearUIText.Invoke("ClearUI", 5f);
        audioSource.PlayOneShot(pickupSound);
        Destroy(this.gameObject);
    }
}
