using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroCutScene : MonoBehaviour
{
   public GameObject aSyncLoaderGameObject;
   private ASyncLoader aSyncLoader;
    private VideoPlayer videoPlayer;
    double videoLength;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.length;
        aSyncLoader = aSyncLoaderGameObject.GetComponent<ASyncLoader>();

    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength <= 0)
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
            aSyncLoader.LoadLevelBtn("VerticalSlice");
        }

        if(Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
             aSyncLoader.LoadLevelBtn("VerticalSlice");
        }
    }
}
