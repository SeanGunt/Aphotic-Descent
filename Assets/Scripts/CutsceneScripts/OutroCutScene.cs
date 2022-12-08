using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OutroCutScene : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    float videoLength = 62f;
    bool hasStarted;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = 62f;
    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength < 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScreen");
        }

        if(Input.anyKeyDown && videoLength < 58f)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScreen");
        }
    }
}
