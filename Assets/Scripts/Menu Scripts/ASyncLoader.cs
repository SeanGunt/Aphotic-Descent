using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
   [SerializeField] private GameObject loadingScreen;
   [SerializeField] private GameObject videoPlayerGM;

   private void Awake()
   {
        loadingScreen.SetActive(false);
   }
   public void LoadLevelBtn(string levelToLoad)
   {
        videoPlayerGM.SetActive(false);
        loadingScreen.SetActive(true);

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