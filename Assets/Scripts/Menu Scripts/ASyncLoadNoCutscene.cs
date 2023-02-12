using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoadNoCutscene : MonoBehaviour
{
     [Header ("Slider")]
   [SerializeField] private GameObject loadingScreen;
   
    [Header ("Slider")]
   [SerializeField] private Slider loadingSlider;

   public void LoadLevelBtn(string levelToLoad)
   {
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