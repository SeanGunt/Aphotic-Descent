using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
     [Header ("Slider")]
   [SerializeField] private GameObject loadingScreen;
   [SerializeField] private GameObject videoPlayerGM;
   
    [Header ("Slider")]
   [SerializeField] private Slider loadingSlider;

   private void Awake()
   {
    loadingScreen.SetActive(false);
   }
   public void LoadLevelBtn(string levelToLoad)
   {
    videoPlayerGM.SetActive(false);
    loadingScreen.SetActive(true);

    //Run the A sync
    StartCoroutine(LoadLevelASync(levelToLoad));
    
   }
    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}