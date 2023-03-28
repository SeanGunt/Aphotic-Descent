using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AnglerEncounter : MonoBehaviour
{
    [SerializeField]private GameObject textHolder;
    [SerializeField]private TMP_Text actualText;
    [SerializeField]private ClearUIText clearUIText;
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip pickupSound;

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
            clearUIText.Invoke("ClearUI", 3f);
            audioSource.PlayOneShot(pickupSound);
            Invoke("EndEncounterCheck", 3f);
        }
        
    }

    private void EndEncounterCheck()
    {
        if (GameDataHolder.subParts == 3)
        {
            actualText.text = "But there's no sub to fix. There's no way out of this.";
            clearUIText.Invoke("ClearUI", 5f);
            Invoke("EndEncounter", 5.1f);
        }
        else
        {
            return;
        }
    }

    private void EndEncounter()
    {
        SceneManager.LoadScene(4);
    }
}
