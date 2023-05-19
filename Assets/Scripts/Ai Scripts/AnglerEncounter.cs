using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Steamworks;

public class AnglerEncounter : MonoBehaviour
{
    [SerializeField]private GameObject textHolder;
    [SerializeField]private TMP_Text actualText;
    [SerializeField]private ClearUIText clearUIText;
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip pickupSound;
    private bool musicChanged;
    
    public void IncreaseSubParts()
    {
        GameDataHolder.subParts++;
        textHolder.SetActive(true);
        if (GameDataHolder.subParts < 3)
        {
            actualText.text = $"Sub parts acquired: {GameDataHolder.subParts}. Find the rest.";
            clearUIText.Invoke("ClearUI", 5f);
            audioSource.PlayOneShot(pickupSound);
            EndEncounterCheck();
        }
        else if (GameDataHolder.subParts == 3)
        {
            actualText.text = "All sub parts found";
            audioSource.PlayOneShot(pickupSound);
            Invoke("EndEncounterCheck", 3.15f);
        }
        
    }

    private void Update()
    {
        if (GameDataHolder.subParts >= 1 && !musicChanged)
        {
            BGMManager.instance.SwitchBGMFade(16);
            musicChanged = true;
        }
    }

    private void EndEncounterCheck()
    {
        if (GameDataHolder.subParts == 3)
        {
            actualText.text = "But the sub is irreparable. There's no way out of this.";
            clearUIText.Invoke("ClearUI", 5f);
            SteamUserStats.SetAchievement("Game_Completed");
            SteamUserStats.StoreStats();
            Invoke("EndEncounter", 5.15f);
            Debug.Log("EncounterEndReached");
        }
        else
        {
            Debug.Log("Not enough parts yet.");
            return;
        }
    }

    private void EndEncounter()
    {
        SceneManager.LoadScene(6);
        
    }
}
