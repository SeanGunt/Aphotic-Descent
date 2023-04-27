using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    double videoLength = 62f;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.length;
    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength < 0)
        {
            LoadWinScreen();
        }
    }

    public void LoadWinScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
