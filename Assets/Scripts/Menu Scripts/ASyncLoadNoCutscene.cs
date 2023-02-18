using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoadNoCutscene : MonoBehaviour
{
   [SerializeField] private GameObject loadingScreen;

   public void LoadLevelBtn(string levelToLoad)
   {
        StartCoroutine(LoadLevelASync(levelToLoad));
   }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
}