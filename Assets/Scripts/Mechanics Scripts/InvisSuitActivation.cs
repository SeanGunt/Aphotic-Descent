using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvisSuitActivation : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI tutText;
    public GameObject tutTextObj;
    [SerializeField] private ClearUIText clearUIText;
    public void UpgradeSuit(InvisibilityMechanic controller)
    {
        GameDataHolder.invisibilityAcquired = true;
        clearUIText.CancelInvoke();
        clearUIText.Invoke("ClearUI", 4);
        tutTextObj.SetActive(true);
        tutText.text = "You have been gifted the power of Invisibility! Press Q To Go Invisible";
        controller.SetInvisUIActive();
        DataPersistenceManager.instance.SaveGame();
    }
}
