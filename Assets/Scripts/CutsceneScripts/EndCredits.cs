using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            LoadWinScreen();
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                LoadWinScreen();
        }
    }

    public void LoadWinScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
